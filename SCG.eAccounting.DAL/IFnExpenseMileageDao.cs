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
    public interface IFnExpenseMileageDao : IDao<FnExpenseMileage, long>
    {
        void Persist(DataTable dtExpenseMileage);
    }
}
