using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SCG.eAccounting.DAL;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;
using System.Data;

namespace SCG.eAccounting.DAL.Hibernate
{
    public class FnExpenseMileageItemDao : NHibernateDaoBase<FnExpenseMileageItem, long>, IFnExpenseMileageItemDao
    {
        #region Save
        public void Persist(DataTable dtExpenseMileageItem)
        {
            NHibernateAdapter<FnExpenseMileageItem, long> adapter = new NHibernateAdapter<FnExpenseMileageItem, long>();
            adapter.UpdateChange(dtExpenseMileageItem, ScgeAccountingDaoProvider.FnExpenseMileageItemDao);
        }
        #endregion
    }
}
