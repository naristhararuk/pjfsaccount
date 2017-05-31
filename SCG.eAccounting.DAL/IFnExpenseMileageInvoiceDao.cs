using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using NHibernate;
using SCG.eAccounting.DTO;
using System.Data;

namespace SCG.eAccounting.DAL
{
    public interface IFnExpenseMileageInvoiceDao : IDao<FnExpenseMileageInvoice, long>
    {
        void Persist(DataTable dtExpenseMileageInvoice);
    }
}
