using System;
using System.Collections.Generic;
using System.Text;
using Spring.Data.NHibernate.Generic;
using NHibernate;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SS.Standard.Data.NHibernate.Query.Callback
{
    public class CountByCriteriaCallback : IHibernateCallback<int>
    {
        private QueryPartCollector queryPartCollector;
        private HqlCreator hqlCreator;

        public CountByCriteriaCallback(HqlCreator hqlCreator, QueryPartCollector queryPartCollector)
        {
            this.hqlCreator = hqlCreator;
            this.queryPartCollector = queryPartCollector;
        }

        public int DoInHibernate(ISession session)
        {
            String selectClause = " select count(*) ";
            String fromClause = hqlCreator.CreateFromClause(queryPartCollector);
            String whereClause = hqlCreator.CreateWhereClause(queryPartCollector);

            StringBuilder hqlBuilder = new StringBuilder();
            hqlBuilder.Append(selectClause);
            hqlBuilder.Append(fromClause);
            hqlBuilder.Append(whereClause);

            IQuery query = session.CreateQuery(hqlBuilder.ToString());
            hqlCreator.FillParameters(query, queryPartCollector);

            return Convert.ToInt32(query.UniqueResult());
        }
    }
}
