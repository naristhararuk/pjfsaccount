using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Query;

using SS.DB.DTO;
using NHibernate;

namespace SS.DB.Query
{
    public interface IDbExchangeRateQuery : IQuery<DbExchangeRate, short> 
    {
        ICriteria FindExchangeRateByCurrencyId(short currencyId);
        IList<DbExchangeRate> GetExchangeList(short currencyId, int firstResult, int maxResult, string sortExpression);
        int CountByExchangeCriteria(short currencyId);
    }
}
