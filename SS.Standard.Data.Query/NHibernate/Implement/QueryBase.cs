using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;
using SS.Standard.Data.Query.NHibernate;


namespace SS.Standard.Data.Query.NHibernate.Implement
{

    public abstract class QueryBase<TDomain, TId> : IQuery<TDomain, TId>
    {
        public abstract IQuery<TDomain, TId> GetBaseQuery();

        public virtual TDomain FindByIdentity(TId id)
        {
            return GetBaseQuery().FindByIdentity(id);
        }

        public virtual TDomain FindProxyByIdentity(TId id)
        {
            return GetBaseQuery().FindProxyByIdentity(id);
        }

        public virtual IList<TDomain> FindAll()
        {
            return GetBaseQuery().FindAll();
        }

        public virtual bool FindExisting(TDomain domain)
        {
            return GetBaseQuery().FindExisting(domain);
        }

        public virtual IList<TDomain> FindByExample(TDomain domain)
        {
            return GetBaseQuery().FindByExample(domain);
        }

        public IList<TDomain> FindByCriteria(QueryPartCollector queryPartCollector)
        {
            return GetBaseQuery().FindByCriteria(queryPartCollector);
        }

        public int CountByCriteria(QueryPartCollector queryPartCollector)
        {
            return GetBaseQuery().CountByCriteria(queryPartCollector);
        }

    }
}
