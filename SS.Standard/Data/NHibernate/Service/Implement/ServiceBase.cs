using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SS.Standard.Data.Service.Implement
{
    public abstract class ServiceBase<TDomain, TId> : IService<TDomain, TId>
    {
        public abstract IDao<TDomain, TId> GetBaseDao();

        public virtual TDomain FindByIdentity(TId id)
        {
            return GetBaseDao().FindByIdentity(id);
        }

        public virtual TDomain FindProxyByIdentity(TId id)
        {
            return GetBaseDao().FindProxyByIdentity(id);
        }

        public virtual IList<TDomain> FindAll()
        {
            return GetBaseDao().FindAll();
        }

        public virtual bool FindExisting(TDomain domain)
        {
            return GetBaseDao().FindExisting(domain);
        }

        public virtual TId Save(TDomain domain)
        {
            return (TId)GetBaseDao().Save(domain);
        }

        public virtual void SaveOrUpdate(TDomain domain)
        {
            GetBaseDao().SaveOrUpdate(domain);
        }

        public virtual void Update(TDomain domain)
        {
            GetBaseDao().Update(domain);
        }

        public virtual void Delete(TDomain domain)
        {
            GetBaseDao().Delete(domain);
        }

        public virtual IList<TDomain> FindByExample(TDomain domain)
        {
            return GetBaseDao().FindByExample(domain);
        }

        public IList<TDomain> FindByCriteria(QueryPartCollector queryPartCollector)
        {
            return GetBaseDao().FindByCriteria(queryPartCollector);
        }

        public int CountByCriteria(QueryPartCollector queryPartCollector)
        {
            return GetBaseDao().CountByCriteria(queryPartCollector);
        }

    }
}
