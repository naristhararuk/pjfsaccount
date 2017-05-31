using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.DataSet;
using System.Data;


namespace SCG.eAccounting.DAL
{
    public interface IExpensesMPADao : IDao<FnExpenseMPA, long>
    {
        long Persist(DataTable dtExpenseMPA);
    }
}
