using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SCG.eAccounting.DTO;
using SCG.eAccounting.DAL;
using SCG.eAccounting.DTO.DataSet;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;

namespace SCG.eAccounting.DAL.Hibernate
{
	public class AvAdvanceItemDao : NHibernateDaoBase<AvAdvanceItem, long>, IAvAdvanceItemDao
    {
        #region Save AvAdvanceItem
        public void Persist(AdvanceDataSet.AvAdvanceItemDataTable dtAvDocument)
        {
            #region comment
            //AdvanceDataSet.AvAdvanceItemDataTable deleteTable =
            //    (AdvanceDataSet.AvAdvanceItemDataTable)dtAvDocument.GetChanges(DataRowState.Deleted);
            //AdvanceDataSet.AvAdvanceItemDataTable updateTable =
            //    (AdvanceDataSet.AvAdvanceItemDataTable)dtAvDocument.GetChanges(DataRowState.Modified);
            //AdvanceDataSet.AvAdvanceItemDataTable insertTable =
            //    (AdvanceDataSet.AvAdvanceItemDataTable)dtAvDocument.GetChanges(DataRowState.Added);

            //long avItemID = 0;

            //// Delete AvAdvanceItem.
            //if (deleteTable != null)
            //{
            //    foreach (AdvanceDataSet.AvAdvanceItemRow avRow in deleteTable)
            //    {
            //        long avID = Convert.ToInt64(((DataRow)avRow)["AdvanceID", DataRowVersion.Original].ToString());
            //        AvAdvanceItem avDocument = new AvAdvanceItem(avID);
            //        this.Delete(avDocument);
            //    }
            //}

            //// Update AvAdvanceItem.
            //if (updateTable != null)
            //{
            //    foreach (AdvanceDataSet.AvAdvanceItemRow avRow in updateTable)
            //    {
            //        AvAdvanceItem avDocument = new AvAdvanceItem(avRow);
            //        this.SaveOrUpdate(avDocument);
            //    }
            //}

            //// Insert AvAdvanceItem.
            //if (insertTable != null)
            //{
            //    foreach (AdvanceDataSet.AvAdvanceItemRow avRow in insertTable)
            //    {
            //        AvAdvanceItem avDocument = new AvAdvanceItem(avRow);
            //        this.Save(avDocument);
            //    }
            //}
            #endregion
            NHibernateAdapter<AvAdvanceItem, long> adapter = new NHibernateAdapter<AvAdvanceItem, long>();
            adapter.UpdateChange(dtAvDocument, ScgeAccountingDaoProvider.AvAdvanceItemDao);
           
        }
        #endregion
	}
}
