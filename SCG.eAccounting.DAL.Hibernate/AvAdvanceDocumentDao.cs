using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.DataSet;
using SCG.eAccounting.DAL;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;

namespace SCG.eAccounting.DAL.Hibernate
{
	public class AvAdvanceDocumentDao : NHibernateDaoBase<AvAdvanceDocument, long>, IAvAdvanceDocumentDao
	{
        #region Save avDocument
        public long Persist(AdvanceDataSet.AvAdvanceDocumentDataTable dtAvDocument)
        {
            #region comment
            //AdvanceDataSet.AvAdvanceDocumentDataTable deleteTable =
            //    (AdvanceDataSet.AvAdvanceDocumentDataTable)dtAvDocument.GetChanges(DataRowState.Deleted);
            //AdvanceDataSet.AvAdvanceDocumentDataTable updateTable =
            //    (AdvanceDataSet.AvAdvanceDocumentDataTable)dtAvDocument.GetChanges(DataRowState.Modified);
            //AdvanceDataSet.AvAdvanceDocumentDataTable insertTable =
            //    (AdvanceDataSet.AvAdvanceDocumentDataTable)dtAvDocument.GetChanges(DataRowState.Added);

            //long avDocumentID = 0;

            //// Delete AvAdvanceDocument.
            //if (deleteTable != null)
            //{
            //    foreach (AdvanceDataSet.AvAdvanceDocumentRow avRow in deleteTable)
            //    {
            //        long avID = Convert.ToInt64(((DataRow)avRow)["AdvanceID", DataRowVersion.Original].ToString());
            //        AvAdvanceDocument avDocument = new AvAdvanceDocument(avID);
            //        this.Delete(avDocument);
            //    }
            //}

            //// Update AvAdvanceDocument.
            //if (updateTable != null)
            //{
            //    AvAdvanceDocument avDocument = new AvAdvanceDocument((AdvanceDataSet.AvAdvanceDocumentRow)updateTable.Rows[0]);
            //    avDocumentID = avDocument.AdvanceID;
            //    this.SaveOrUpdate(avDocument);
            //}

            //// Insert AvAdvanceDocument.
            //if (insertTable != null)
            //{
            //    AvAdvanceDocument avDocument = new AvAdvanceDocument((AdvanceDataSet.AvAdvanceDocumentRow)insertTable.Rows[0]);
            //    avDocumentID = this.Save(avDocument);

            //    #region Update New avDocumentID to Dataset
            //    dtAvDocument.AdvanceIDColumn.ReadOnly = false;
            //    dtAvDocument.Rows[0].BeginEdit();
            //    dtAvDocument.Rows[0][dtAvDocument.AdvanceIDColumn.ColumnName] = avDocumentID;
            //    dtAvDocument.Rows[0].EndEdit();
            //    dtAvDocument.AdvanceIDColumn.ReadOnly = true;
            //    #endregion
            //}
            //return avDocumentID;
            #endregion
            NHibernateAdapter<AvAdvanceDocument, long> adapter = new NHibernateAdapter<AvAdvanceDocument, long>();
            adapter.UpdateChange(dtAvDocument, ScgeAccountingDaoProvider.AvAdvanceDocumentDao);
            return dtAvDocument.Rows[0].Field<long>(dtAvDocument.Columns["AdvanceID"]);
        }
        #endregion

	}
}
