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
using SCG.eAccounting.DTO.DataSet;
using System.Data;

namespace SCG.eAccounting.DAL.Hibernate
{
	public class FnExpenseAdvanceDao : NHibernateDaoBase<FnExpenseAdvance, long>, IFnExpenseAdvanceDao
	{
        #region Save 
        public long Persist(DataTable dtExpenseAdvance)
        {
            NHibernateAdapter<FnExpenseAdvance, long> adapter = new NHibernateAdapter<FnExpenseAdvance, long>();
            adapter.UpdateChange(dtExpenseAdvance, ScgeAccountingDaoProvider.FnExpenseAdvanceDao);

            //return dtExpenseAdvance.Rows[0].Field<long>(dtExpenseAdvance.Columns["FnExpenseAdvanceID"]);
            return 0;
        }
        #endregion
	}
}