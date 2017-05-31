using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Query;
using SS.Standard.WorkFlow.DTO;

using NHibernate;

namespace SS.Standard.WorkFlow.Query
{
    public interface IWorkFlowTypeDocumentTypeQuery : IQuery<WorkFlowTypeDocumentType, long> 
    {
        WorkFlowTypeDocumentType GetByWorkFlowTypeID_DocumentTypeID(int workFlowTypeID , int documentTypeID);
    }
}
