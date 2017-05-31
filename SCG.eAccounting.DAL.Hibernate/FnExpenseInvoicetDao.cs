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
	public class FnExpenseInvoiceDao : NHibernateDaoBase<FnExpenseInvoice, long>, IFnExpenseInvoiceDao
	{
        #region Save
        public void Persist(DataTable dtExpenseInvoice)
        {
            NHibernateAdapter<FnExpenseInvoice, long> adapter = new NHibernateAdapter<FnExpenseInvoice, long>();
            adapter.UpdateChange(dtExpenseInvoice, ScgeAccountingDaoProvider.FnExpenseInvoiceDao);

            //return dtExpenseDocument.Rows[0].Field<long>(dtExpenseDocument.Columns["ExpenseID"]);
        }
        #endregion
	}
}