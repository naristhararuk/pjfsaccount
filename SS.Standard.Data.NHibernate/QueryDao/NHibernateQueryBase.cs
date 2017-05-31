using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Reflection;

using NHibernate;
using NHibernate.Expression;

using Spring.Data.NHibernate.Generic.Support;

using SS.Standard.Data.NHibernate.QueryCreator;
using SS.Standard.Data.NHibernate.QueryCreator.Descriptor;
using SS.Standard.Data.NHibernate.Query;
using SS.Standard.Data.NHibernate.QueryDao.Callback;
//using SS.Standard.Data.Query;
using Spring.Data.NHibernate;

namespace SS.Standard.Data.NHibernate.QueryDao
{
    public abstract class NHibernateQueryBase<TDomain, TId> : HibernateDaoSupport, IQuery<TDomain, TId>
    {
        private FindByExampleDescriptor findByExampleDescriptor;
        private HqlCreator baseHqlCreator;

        public HqlCreator BaseHqlCreator
        {
            set { this.baseHqlCreator = value; }
        }

        public FindByExampleDescriptor FindByExampleDescriptor
        {
            set { findByExampleDescriptor = value; }
        }

        public virtual TDomain FindByIdentity(TId id)
        {
            return HibernateTemplate.Get<TDomain>(id);
        }

        public virtual TDomain FindProxyByIdentity(TId id)
        {
            return HibernateTemplate.Load<TDomain>(id);
        }

        public virtual IList<TDomain> FindAll()
        {
            return HibernateTemplate.LoadAll<TDomain>();
        }

        public virtual bool FindExisting(TDomain domain)
        {
            return HibernateTemplate.Contains(domain);
        }

        public virtual IList<TDomain> FindByExample(TDomain domain)
        {
            FindByExampleCallback<TDomain> findByExampleCallback = new FindByExampleCallback<TDomain>(domain, findByExampleDescriptor);
            return HibernateTemplate.ExecuteFind<TDomain>(findByExampleCallback);
        }

        public IList<TDomain> FindByCriteria(QueryPartCollector queryPartCollector)
        {
            FindByCriteriaCallback<TDomain> findByCriteriaCallback = new FindByCriteriaCallback<TDomain>(baseHqlCreator, queryPartCollector);
            return HibernateTemplate.ExecuteFind<TDomain>(findByCriteriaCallback);
        }

        public int CountByCriteria(QueryPartCollector queryPartCollector)
        {
            CountByCriteriaCallback countByCriteriaCallback = new CountByCriteriaCallback(baseHqlCreator, queryPartCollector);
            return HibernateTemplate.Execute<int>(countByCriteriaCallback);
        }

        protected virtual ISession GetCurrentSession()
        {
            return DoGetSession(true);
        }

    }
}
