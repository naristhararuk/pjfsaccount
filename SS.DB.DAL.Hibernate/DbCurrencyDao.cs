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
    public partial class DbCurrencyDao : NHibernateDaoBase<DbCurrency, short>, IDbCurrencyDao
    {
        public DbCurrencyDao()
        {
        }
        //public void DeleteByCurrencyId(short currencyId)
        //{
        //    GetCurrentSession()
        //        .Delete("from DbCurrency c1 where c1.Currency.CurrencyID = :currencyId "
        //        , new object[] { currencyId }
        //        , new NHibernate.Type.IType[] { NHibernateUtil.Int16});
        //}
        public bool IsDuplicateSymbol(DbCurrency currency)
        {
            IList<DbCurrency> list = GetCurrentSession().CreateQuery("from DbCurrency c where c.CurrencyID <> :CurrencyID and c.Symbol = :Symbol")
                  .SetInt64("CurrencyID", currency.CurrencyID)
                  .SetString("Symbol",currency.Symbol)
                  .List<DbCurrency>();
            if (list.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
