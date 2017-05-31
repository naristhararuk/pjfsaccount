using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SS.DB.DTO;
using SS.DB.DAL;
using NHibernate;
using NHibernate.Expression;

namespace SS.DB.DAL.Hibernate
{
    public partial class DbExchangeRateDao : NHibernateDaoBase<DbExchangeRate, short>, IDbExchangeRateDao
    {
        public DbExchangeRateDao()
        {
        }
        //public bool IsDuplicateSymbol(DbCurrency currency)
        //{
        //    IList<DbCurrency> list = GetCurrentSession().CreateQuery("from DbCurrency c where c.CurrencyID <> :CurrencyID and c.Symbol = :Symbol")
        //          .SetInt64("CurrencyID", currency.CurrencyID)
        //          .SetString("Symbol",currency.Symbol)
        //          .List<DbCurrency>();
        //    if (list.Count > 0)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
    }
}
