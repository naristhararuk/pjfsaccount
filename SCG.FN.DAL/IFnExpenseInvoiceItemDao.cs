using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SCG.FN.DTO;
using NHibernate;

namespace SCG.FN.DAL
{
    public interface IFnExpenseInvoiceItemDao : IDao<FnExpenseInvoiceItem, long>
    {

    }
}
