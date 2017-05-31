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
    public class FnRemittanceAdvanceDao : NHibernateDaoBase<FnRemittanceAdvance, long>, IFnRemittanceAdvanceDao
	{
        public void Persist(FnRemittanceDataset.FnRemittanceAdvanceDataTable dtRemittanceAdvance)
        {
            NHibernateAdapter<FnRemittanceAdvance, long> adapter = new NHibernateAdapter<FnRemittanceAdvance, long>();
            adapter.UpdateChange(dtRemittanceAdvance, ScgeAccountingDaoProvider.FnRemittanceAdvanceDao);
            #region Old 24-March-2009
            //FnRemittanceDataset.FnRemittanceAdvanceDataTable insertTable = (FnRemittanceDataset.FnRemittanceAdvanceDataTable)dtRemittanceAdvance.GetChanges(DataRowState.Added);
            //FnRemittanceDataset.FnRemittanceAdvanceDataTable updateTable = (FnRemittanceDataset.FnRemittanceAdvanceDataTable)dtRemittanceAdvance.GetChanges(DataRowState.Modified);
            //FnRemittanceDataset.FnRemittanceAdvanceDataTable deleteTable = (FnRemittanceDataset.FnRemittanceAdvanceDataTable)dtRemittanceAdvance.GetChanges(DataRowState.Deleted);

            //if (insertTable != null)
            //{
            //    foreach (FnRemittanceDataset.FnRemittanceAdvanceRow row in dtRemittanceAdvance)
            //    {
            //        FnRemittanceAdvance remittanceAdvance = new FnRemittanceAdvance(row);
            //        this.Save(remittanceAdvance);
            //    }
            //}
            //if (updateTable != null)
            //{
            //    foreach (FnRemittanceDataset.FnRemittanceAdvanceRow row in dtRemittanceAdvance)
            //    {
            //        FnRemittanceAdvance remittanceAdvance = new FnRemittanceAdvance(row);
            //        this.SaveOrUpdate(remittanceAdvance);
            //    }
            //}
            //if (deleteTable != null)
            //{
            //    foreach (FnRemittanceDataset.FnRemittanceAdvanceRow row in dtRemittanceAdvance)
            //    {
            //        long remittanceID = Convert.ToInt64(((DataRow)row)["RemittanceAdvanceID", DataRowVersion.Original].ToString());
            //        FnRemittanceAdvance remittanceAdvance = new FnRemittanceAdvance(row);
            //        this.Delete(remittanceAdvance);
            //    }
            //}
            #endregion

        }
	}
}
