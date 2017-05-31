using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SS.Standard.Data.Service
{
    public interface IService<TDomain, TId>
    {
        bool FindExisting(TDomain domain);
        TDomain FindByIdentity(TId id);
        TDomain FindProxyByIdentity(TId id);
        IList<TDomain> FindAll();
        TId Save(TDomain domain);
        void SaveOrUpdate(TDomain domain);
        void Update(TDomain domain);
        void Delete(TDomain domain);
        IList<TDomain> FindByExample(TDomain domain);
        IList<TDomain> FindByCriteria(QueryPartCollector queryPartCollector);
        int CountByCriteria(QueryPartCollector queryPartCollector);
    }
}
