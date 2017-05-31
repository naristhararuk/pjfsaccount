using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SS.Standard.Data.NHibernate.QueryDao;

using SS.DB.DTO;
using SS.DB.DTO.ValueObject;
using SS.DB.Query;
using NHibernate;
using NHibernate.Transform;
using SS.Standard.Security;
using SS.Standard.Data.NHibernate.QueryCreator;
using NHibernate.Expression;

namespace SS.DB.Query.Hibernate
{
    public class DbExchangeRateQuery : NHibernateQueryBase<DbExchangeRate, short>, IDbExchangeRateQuery  
    {
        public ICriteria FindExchangeRateByCurrencyId(short currencyId)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(DbExchangeRate), "ex");
            criteria.Add(Expression.Eq("ex.Currency.CurrencyID",currencyId));
            IList<DbExchangeRate> dbExchangeRateList =  criteria.List<DbExchangeRate>();
            foreach (DbExchangeRate dbExchangeRate in dbExchangeRateList)
            {
                Console.WriteLine(dbExchangeRate.Currency);
                Console.WriteLine(dbExchangeRate.Currency.CurrencyID);
            }
            return criteria;
        }
        public IList<DbExchangeRate> GetExchangeList(short currencyId, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<DbExchangeRate>(SsDbQueryProvider.DbExchangeRateQuery, "FindExchangeRateByCurrencyId", new object[] { currencyId }, firstResult, maxResult, sortExpression);
        }
        public int CountByExchangeCriteria(short currencyId)
        {
            return NHibernateQueryHelper.CountByCriteria(SsDbQueryProvider.DbExchangeRateQuery, "FindExchangeRateByCurrencyId", new object[] { currencyId });
        }
    }
}
