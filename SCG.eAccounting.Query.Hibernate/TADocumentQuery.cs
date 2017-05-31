using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.QueryDao;
using SCG.eAccounting.DTO;
using SS.Standard.Data.NHibernate.QueryCreator;
using SCG.eAccounting.DTO.ValueObject;
using NHibernate;
using NHibernate.Transform;
using NHibernate.Expression;

namespace SCG.eAccounting.Query.Hibernate
{
    public class TADocumentQuery : NHibernateQueryBase<TADocument, long>, ITADocumentQuery
    {
        #region public TADocument GetTADocumentByDocumentID(long documentID)
        public TADocument GetTADocumentByDocumentID(long documentID)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(TADocument), "t");
            criteria.Add(Expression.Eq("t.DocumentID.DocumentID", documentID));

            return criteria.UniqueResult<TADocument>();
        }
        #endregion public TADocument GetTADocumentByDocumentID(long documentID)        
    }
}
