using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.QueryDao;

using SS.Standard.WorkFlow.DTO;

using NHibernate;
using NHibernate.Transform;
using NHibernate.Expression;

namespace SS.Standard.WorkFlow.Query.Hibernate
{
    public class DocumentTypeQuery : NHibernateQueryBase<DocumentType , int> , IDocumentTypeQuery
    {
        public DocumentType FindByIdentityRunningDocument(int documentTypeID)
        {
            DocumentType documentType = GetCurrentSession().CreateQuery("from DocumentType where DocumentTypeID = :DocumentTypeID and active = '1'")
            .SetInt32("DocumentTypeID", documentTypeID)
            .UniqueResult<DocumentType>();
            
            GetCurrentSession().Refresh(documentType, LockMode.Upgrade);

            return documentType;
        }
    }
}
