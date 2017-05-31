using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SCG.eAccounting.DTO;
using SCG.eAccounting.DAL;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;
using SCG.eAccounting.DTO.DataSet;
using System.Data;

namespace SCG.eAccounting.DAL.Hibernate
{
    public class FnRemittanceItemDao : NHibernateDaoBase<FnRemittanceItem, long>, IFnRemittanceItemDao
	{
        public void Persist(FnRemittanceDataset.FnRemittanceItemDataTable dtRemittanceItem)
        {
            NHibernateAdapter<FnRemittanceItem, long> adapter = new NHibernateAdapter<FnRemittanceItem, long>();
            adapter.UpdateChange(dtRemittanceItem, ScgeAccountingDaoProvider.FnRemittanceItemDao);
            #region Old 24-March-2009
            //FnRemittanceDataset.FnRemittanceItemDataTable insertTable = (FnRemittanceDataset.FnRemittanceItemDataTable)dtRemittanceItem.GetChanges(DataRowState.Added);
            //FnRemittanceDataset.FnRemittanceItemDataTable updateTable = (FnRemittanceDataset.FnRemittanceItemDataTable)dtRemittanceItem.GetChanges(DataRowState.Modified);
            //FnRemittanceDataset.FnRemittanceItemDataTable deleteTable = (FnRemittanceDataset.FnRemittanceItemDataTable)dtRemittanceItem.GetChanges(DataRowState.Deleted);

            //if (insertTable != null)
            //{
            //    foreach (FnRemittanceDataset.FnRemittanceItemRow row in insertTable)
            //    {
            //        FnRemittanceItem remittanceItem = new FnRemittanceItem(row);
            //        this.Save(remittanceItem);
            //    }

            //}
            //if (updateTable != null)
            //{
            //    foreach (FnRemittanceDataset.FnRemittanceItemRow row in insertTable)
            //    {
            //        FnRemittanceItem remittanceItem = new FnRemittanceItem(row);
            //        this.SaveOrUpdate(remittanceItem);
            //    }
            //}
            //if (deleteTable != null)
            //{
            //    foreach (FnRemittanceDataset.FnRemittanceItemRow row in insertTable)
            //    {
            //        long remittanceID = Convert.ToInt64(((DataRow)row)["RemittanceItemID", DataRowVersion.Original].ToString());
            //        FnRemittanceItem remittanceItem = new FnRemittanceItem(row);
            //        this.Delete(remittanceItem);
            //    }
            //}
            #endregion
        }
	}
}
