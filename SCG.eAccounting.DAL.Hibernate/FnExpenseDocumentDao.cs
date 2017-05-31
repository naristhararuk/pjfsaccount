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
	public class FnExpenseDocumentDao : NHibernateDaoBase<FnExpenseDocument, long>, IFnExpenseDocumentDao
	{
        #region Save 
        public long Persist(DataTable dtExpenseDocument)
        {
            NHibernateAdapter<FnExpenseDocument, long> adapter = new NHibernateAdapter<FnExpenseDocument, long>();
            adapter.UpdateChange(dtExpenseDocument, ScgeAccountingDaoProvider.FnExpenseDocumentDao);

            return dtExpenseDocument.Rows[0].Field<long>(dtExpenseDocument.Columns["ExpenseID"]);
        }
        #endregion
	}
}