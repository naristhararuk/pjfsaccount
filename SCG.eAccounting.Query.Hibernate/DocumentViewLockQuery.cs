using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.QueryCreator;
using SS.Standard.Data.NHibernate.QueryDao;

using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;
using NHibernate.Expression;

namespace SCG.eAccounting.Query.Hibernate
{
    public class DocumentViewLockQuery : NHibernateQueryBase<DocumentViewLock, long>, IDocumentViewLockQuery
    {

        public DocumentViewLock GetDocumentViewLockByDocumentID(long documentID)
        {
            return GetCurrentSession().CreateQuery("from DocumentViewLock where DocumentID = :DocumentID")
                .SetInt64("DocumentID", documentID)
                .UniqueResult<DocumentViewLock>();
        }

    }
}

