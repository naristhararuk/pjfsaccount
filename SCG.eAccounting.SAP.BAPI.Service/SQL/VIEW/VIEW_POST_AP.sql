USE [eAccounting]
GO
/****** Object:  View [dbo].[VIEW_POST_AP]    Script Date: 04/20/2009 16:36:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VIEW_POST_AP]
AS
SELECT     dbo.BAPIACAP09.DOC_KIND, dbo.BAPIACAP09.DOC_ID, dbo.BAPIACAP09.DOC_SEQ, dbo.BAPIACAP09.ITEMNO_ACC, 
                      dbo.BAPIACAP09.VENDOR_NO AS Account, ISNULL(dbo.BAPIACAP09.TAX_CODE, N'NV') AS TAX_CODE, RIGHT(dbo.BAPIACAP09.BLINE_DATE, 2) 
                      + '/' + SUBSTRING(dbo.BAPIACAP09.BLINE_DATE, 3, 2) + '/' + LEFT(dbo.BAPIACAP09.BLINE_DATE, 4) AS BaseDate, dbo.BAPIACAP09.PMNTTRMS, 
                      dbo.BAPIACAP09.PYMT_METH, dbo.BAPIACCR09.CURRENCY, dbo.BAPIACCR09.AMT_DOCCUR, '' AS CostCenter, '' AS InterOrder, 
                      CASE WHEN BAPIACAP09.VENDOR_NO = 'PCADVCL' THEN ISNULL
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

