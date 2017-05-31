using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.DTO;
using SS.Standard.Data.NHibernate.QueryDao;
using NHibernate;
using NHibernate.Expression;

namespace SCG.eAccounting.Query.Hibernate
{
    public class MPAItemQuery : NHibernateQueryBase<MPAItem, long> , IMPAItemQuery
    {
        public IList<MPAItem> FindMPAItemByMPADocumentID(long MPADocumentID)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(MPAItem), "t");
            criteria.Add(Expression.Eq("t.MPADocumentID.MPADocumentID", MPADocumentID));

            return criteria.List<MPAItem>();
        }
    }
}
