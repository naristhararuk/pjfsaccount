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
	public class FnExpenseRemittanceDao : NHibernateDaoBase<FnExpenseRemittance, long>, IFnExpenseRemittanceDao
	{
        #region Save 
        public long Persist(DataTable dtExpenseRemittance)
        {
            NHibernateAdapter<FnExpenseRemittance, long> adapter = new NHibernateAdapter<FnExpenseRemittance, long>();
            adapter.UpdateChange(dtExpenseRemittance, ScgeAccountingDaoProvider.FnExpenseRemittanceDao);

            //return dtExpenseRemittance.Rows[0].Field<long>(dtExpenseRemittance.Columns["ExpenseRemittanceID"]);
            return 0;
        }
        #endregion
	}
}