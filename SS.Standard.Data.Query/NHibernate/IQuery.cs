using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SS.Standard.Data.Query.NHibernate
{

    public partial interface IQuery<TDomain, TId>
    {
        bool FindExisting(TDomain domain);
        TDomain FindByIdentity(TId id);
        TDomain FindProxyByIdentity(TId id);
        IList<TDomain> FindAll();
        IList<TDomain> FindByExample(TDomain domain);
        IList<TDomain> FindByCriteria(QueryPartCollector queryPartCollector);
        int CountByCriteria(QueryPartCollector queryPartCollector);
    }
}
