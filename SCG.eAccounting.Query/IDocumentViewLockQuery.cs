using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.Query.NHibernate;

using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;
using NHibernate;

namespace SCG.eAccounting.Query
{
    public interface IDocumentViewLockQuery : IQuery<DocumentViewLock, long>
    {
        DocumentViewLock GetDocumentViewLockByDocumentID(long documentID);
    }
}
