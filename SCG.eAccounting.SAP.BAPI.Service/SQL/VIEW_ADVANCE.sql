USE [eAccounting]
GO
/****** Object:  View [dbo].[AdvancePostingHead]    Script Date: 03/26/2009 10:20:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[AdvancePostingHead]
AS
SELECT     dbo.AvAdvanceDocument.DocumentID, dbo.AvAdvanceDocument.AdvanceID, dbo.[Document].CompanyID, dbo.[Document].CreDate, 
                      dbo.WorkFlowVerifyResponse.PostingDate, dbo.WorkFlowVerifyResponse.BranchCode
FROM         dbo.[Document] INNER JOIN
                      dbo.AvAdvanceDocument ON dbo.[Document].DocumentID = dbo.AvAdvanceDocument.DocumentID INNER JOIN
                      dbo.WorkFlow ON dbo.[Document].DocumentID = dbo.WorkFlow.DocumentID INNER JOIN
                      dbo.WorkFlowResponse ON dbo.WorkFlow.WorkFlowID = dbo.WorkFlowResponse.WorkFlowID INNER JOIN
                      dbo.WorkFlowVerifyResponse ON dbo.WorkFlowResponse.WorkFlowResponseID = dbo.WorkFlowVerifyResponse.WorkFlowResponseID INNER JOIN
                      dbo.DbCompany ON dbo.[Document].CompanyID = dbo.DbCompany.CompanyID
WHERE     (dbo.WorkFlowResponse.WorkFlowStateEventID = 18) OR
                      (dbo.WorkFlowResponse.WorkFlowStateEventID = 21)

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[62] 4[4] 2[18] 3) )"
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
               Bottom = 312
               Right = 202
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "AvAdvanceDocument"
            Begin Extent = 
               Top = 41
               Left = 717
               Bottom = 156
               Right = 927
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "WorkFlow"
            Begin Extent = 
               Top = 12
               Left = 411
               Bottom = 127
               Right = 574
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "WorkFlowResponse"
            Begin Extent = 
               Top = 172
               Left = 609
               Bottom = 287
               Right = 802
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "WorkFlowVerifyResponse"
            Begin Extent = 
               Top = 168
               Left = 962
               Bottom = 283
               Right = 1176
            End
            DisplayFlags = 280
            TopColumn = 3
         End
         Begin Table = "DbCompany"
            Begin Extent = 
               Top = 181
               Left = 295
               Bottom = 296
               Right = 506
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
         Wid' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'AdvancePostingHead'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'th = 1500
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
' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'AdvancePostingHead'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'AdvancePostingHead'

GO
/****** Object:  View [dbo].[KLA1]    Script Date: 03/26/2009 10:20:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[KLA1]
AS
SELECT     dbo.[Document].DocumentNo, dbo.DbCompany.CompanyCode AS COMP_CODE, dbo.[Document].CreDate AS DOC_DATE, 
                      dbo.WorkFlowVerifyResponse.PostingDate, dbo.WorkFlowVerifyResponse.BranchCode
FROM         dbo.[Document] INNER JOIN
                      dbo.DbCompany ON dbo.[Document].CompanyID = dbo.DbCompany.CompanyID INNER JOIN
                      dbo.AvAdvanceDocument ON dbo.[Document].DocumentID = dbo.AvAdvanceDocument.DocumentID INNER JOIN
                      dbo.WorkFlow ON dbo.[Document].DocumentID = dbo.WorkFlow.DocumentID INNER JOIN
                      dbo.WorkFlowResponse ON dbo.WorkFlow.WorkFlowID = dbo.WorkFlowResponse.WorkFlowID INNER JOIN
                      dbo.WorkFlowVerifyResponse ON dbo.WorkFlowResponse.WorkFlowResponseID = dbo.WorkFlowVerifyResponse.WorkFlowResponseID

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[38] 4[12] 2[31] 3) )"
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
               Left = 263
               Bottom = 297
               Right = 547
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "DbCompany"
            Begin Extent = 
               Top = 34
               Left = 5
               Bottom = 218
               Right = 216
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "AvAdvanceDocument"
            Begin Extent = 
               Top = 6
               Left = 585
               Bottom = 297
               Right = 795
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "WorkFlow"
            Begin Extent = 
               Top = 6
               Left = 833
               Bottom = 256
               Right = 996
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "WorkFlowResponse"
            Begin Extent = 
               Top = 6
               Left = 1034
               Bottom = 397
               Right = 1227
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "WorkFlowVerifyResponse"
            Begin Extent = 
               Top = 294
               Left = 747
               Bottom = 427
               Right = 961
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
         Width =' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'KLA1'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N' 1500
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
         Column = 2160
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
' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'KLA1'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'KLA1'

GO
/****** Object:  View [dbo].[KLA2]    Script Date: 03/26/2009 10:20:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[KLA2]
AS
SELECT     dbo.[Document].DocumentNo, dbo.DbCompany.CompanyCode AS COMP_CODE, dbo.[Document].CreDate AS DOC_DATE, 
                      dbo.WorkFlowVerifyResponse.PostingDate, dbo.WorkFlowVerifyResponse.BranchCode, dbo.DbPB.PBCode, dbo.[Document].RequesterID, 
                      dbo.[Document].ReceiverID, dbo.[Document].CreatorID, dbo.[Document].ApproverID, dbo.WorkFlowVerifyResponse.BaseLineDate, 
                      dbo.DbPaymentMethod.PaymentMethodCode, dbo.DbCurrency.Symbol AS Currency, dbo.AvAdvanceItem.Amount, 
                      dbo.[Document].Subject AS Description, dbo.AvAdvanceDocument.DueDateOfRemittance AS DueDate
FROM         dbo.[Document] INNER JOIN
                      dbo.DbCompany ON dbo.[Document].CompanyID = dbo.DbCompany.CompanyID INNER JOIN
                      dbo.AvAdvanceDocument ON dbo.[Document].DocumentID = dbo.AvAdvanceDocument.DocumentID INNER JOIN
                      dbo.WorkFlow ON dbo.[Document].DocumentID = dbo.WorkFlow.DocumentID INNER JOIN
                      dbo.WorkFlowResponse ON dbo.WorkFlow.WorkFlowID = dbo.WorkFlowResponse.WorkFlowID INNER JOIN
                      dbo.WorkFlowVerifyResponse ON dbo.WorkFlowResponse.WorkFlowResponseID = dbo.WorkFlowVerifyResponse.WorkFlowResponseID INNER JOIN
                      dbo.AvAdvanceItem ON dbo.AvAdvanceDocument.AdvanceID = dbo.AvAdvanceItem.AdvanceDocumentID INNER JOIN
                      dbo.DbPB ON dbo.AvAdvanceDocument.PBID = dbo.DbPB.PBID INNER JOIN
                      dbo.DbPaymentMethod ON dbo.WorkFlowVerifyResponse.PaymentMethodID = dbo.DbPaymentMethod.PaymentMethodID INNER JOIN
                      dbo.DbCurrency ON dbo.AvAdvanceItem.CurrencyID = dbo.DbCurrency.CurrencyID
WHERE     (dbo.WorkFlowResponse.WorkFlowStateEventID = 18) OR
                      (dbo.WorkFlowResponse.WorkFlowStateEventID = 21)

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[70] 4[4] 2[5] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1[50] 4[25] 3) )"
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
         Configuration = "(H (1[66] 3) )"
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
         Configuration = "(H (1[75] 4) )"
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
      ActivePaneConfig = 1
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
               Bottom = 400
               Right = 202
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbCompany"
            Begin Extent = 
               Top = 129
               Left = 164
               Bottom = 289
               Right = 375
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "AvAdvanceDocument"
            Begin Extent = 
               Top = 10
               Left = 467
               Bottom = 378
               Right = 677
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "WorkFlow"
            Begin Extent = 
               Top = 6
               Left = 674
               Bottom = 301
               Right = 837
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "WorkFlowResponse"
            Begin Extent = 
               Top = 6
               Left = 938
               Bottom = 272
               Right = 1131
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "WorkFlowVerifyResponse"
            Begin Extent = 
               Top = 81
               Left = 1159
               Bottom = 317
               Right = 1373
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "AvAdvanceItem"
            Begin Extent = 
               Top = 17
               Left = 736
               Bottom = 362
               Right = 918
    ' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'KLA2'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'        End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbPB"
            Begin Extent = 
               Top = 120
               Left = 319
               Bottom = 334
               Right = 471
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbPaymentMethod"
            Begin Extent = 
               Top = 337
               Left = 707
               Bottom = 498
               Right = 893
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbCurrency"
            Begin Extent = 
               Top = 274
               Left = 838
               Bottom = 468
               Right = 990
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
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 20
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
' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'KLA2'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'KLA2'

GO
/****** Object:  View [dbo].[PostingDate]    Script Date: 03/26/2009 10:20:11 ******/
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
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[52] 4[10] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1[50] 4[25] 3) )"
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
         Configuration = "(H (1[56] 3) )"
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
         Begin Table = "WorkFlow"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 304
               Right = 201
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "WorkFlowResponse"
            Begin Extent = 
               Top = 6
               Left = 257
               Bottom = 274
               Right = 450
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "WorkFlowVerifyResponse"
            Begin Extent = 
               Top = 0
               Left = 565
               Bottom = 282
               Right = 779
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
' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'PostingDate'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'PostingDate'
