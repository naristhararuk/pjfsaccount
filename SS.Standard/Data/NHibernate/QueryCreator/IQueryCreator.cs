using System;
using System.Collections;
using System.Text;
using SS.Standard.Data.NHibernate.QueryParts;

namespace SS.Standard.Data.NHibernate.QueryCreator
{
    public interface IQueryCreator
    {
        string CreateSelectClause(QueryPartCollector queryPartCollector);
        string CreateFromClause(QueryPartCollector queryPartCollector);
        string CreateWhereClause(QueryPartCollector queryPartCollector);
        string CreateOrderByClause(QueryPartCollector queryPartCollector);
        string CreateGroupByClause(QueryPartCollector queryPartCollector);
        string CreateHavingClause(QueryPartCollector queryPartCollector);
    }
}
