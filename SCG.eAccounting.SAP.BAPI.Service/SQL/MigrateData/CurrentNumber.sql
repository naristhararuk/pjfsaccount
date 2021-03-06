SELECT     dbo.BAPIACTX09.DOC_KIND, dbo.BAPIACTX09.DOC_ID, dbo.BAPIACTX09.DOC_SEQ, dbo.BAPIACTX09.ITEMNO_ACC, 
                      dbo.BAPIACTX09.GL_ACCOUNT AS Account, dbo.BAPIACTX09.TAX_CODE, '' AS BaseDate, '' AS PMNTTRMS, '' AS PYMT_METH, '' AS CURRENCY, 

   CONVERT(VARCHAR,CAST(dbo.BAPIACCR09.AMT_DOCCUR AS MONEY),1) AS AMT_DOCCUR

, '' AS CostCenter, '' AS InterOrder, '' AS WHTCode1, '' AS WHTBase1, '' AS WHTCode2, '' AS WHTBase2, 
                      '' AS ALLOC_NMBR, '' AS ITEM_TEXT, 
                      CASE WHEN BAPIACCR09.AMT_DOCCUR >= 0 THEN '40' WHEN BAPIACCR09.AMT_DOCCUR < 0 THEN '50' END AS PK
FROM         dbo.BAPIACTX09 INNER JOIN
                      dbo.BAPIACCR09 ON dbo.BAPIACTX09.DOC_ID = dbo.BAPIACCR09.DOC_ID AND dbo.BAPIACTX09.DOC_SEQ = dbo.BAPIACCR09.DOC_SEQ AND 
                      dbo.BAPIACTX09.ITEMNO_ACC = dbo.BAPIACCR09.ITEMNO_ACC AND dbo.BAPIACTX09.DOC_KIND = dbo.BAPIACCR09.DOC_KIND