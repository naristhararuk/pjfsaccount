using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.QueryDao;

using SS.Standard.WorkFlow.DTO;

using NHibernate;
using NHibernate.Transform;

namespace SS.Standard.WorkFlow.Query.Hibernate
{
    public class WorkFlowTypeDocumentTypeQuery : NHibernateQueryBase<WorkFlowTypeDocumentType, long>, IWorkFlowTypeDocumentTypeQuery
    {
        public WorkFlowTypeDocumentType GetByWorkFlowTypeID_DocumentTypeID(int workFlowTypeID , int documentTypeID)
        {
            return GetCurrentSession().CreateQuery("from WorkFlowTypeDocumentType where WorkFlowTypeID = :WorkFlowTypeID and DocumentTypeID = :DocumentTypeID and Active='1'")
                .SetInt32("WorkFlowTypeID", workFlowTypeID)
                .SetInt32("DocumentTypeID", documentTypeID)
                .UniqueResult<WorkFlowTypeDocumentType>();
        }

    }
}
