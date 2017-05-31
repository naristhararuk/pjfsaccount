using System;
using System.Collections.Generic;
using System.Text;
using Spring.Data.NHibernate.Generic;
using NHibernate;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SS.Standard.Data.NHibernate.Dao.Callback
{
    public class FindByCriteriaCallback<TDomain> : IFindHibernateCallback<TDomain>
    {
        private QueryPartCollector queryPartCollector;
        private HqlCreator hqlCreator;

        public FindByCriteriaCallback(HqlCreator hqlCreator, QueryPartCollector queryPartCollector)
        {
            this.hqlCreator = hqlCreator;
            this.queryPartCollector = queryPartCollector;
        }

        public IList<TDomain> DoInHibernate(ISession session)
        {
            String selectClause = hqlCreator.CreateSelectClause(queryPartCollector);
            String fromClause = hqlCreator.CreateFromClause(queryPartCollector);
            String whereClause = hqlCreator.CreateWhereClause(queryPartCollector);
            String orderByClause = hqlCreator.CreateOrderByClause(queryPartCollector);

            StringBuilder hqlBuilder = new StringBuilder();
            hqlBuilder.Append(selectClause);
            hqlBuilder.Append(fromClause);
            hqlBuilder.Append(whereClause);
            hqlBuilder.Append(orderByClause);

            IQuery query = session.CreateQuery(hqlBuilder.ToString());
            hqlCreator.FillParameters(query, queryPartCollector);

            if (queryPartCollector.FirstResult >= 0 && queryPartCollector.MaxResults >= 0)
            {
                query.SetFirstResult(queryPartCollector.FirstResult);
                query.SetMaxResults(queryPartCollector.MaxResults);
            }

            return query.List<TDomain>();
        }

    }
}
