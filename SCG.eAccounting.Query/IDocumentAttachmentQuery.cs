using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SCG.eAccounting.DTO;

using SS.Standard.Data.Query;
using SS.Standard.Data.Query.NHibernate;
using NHibernate;
using SCG.eAccounting.DTO.ValueObject;

namespace SCG.eAccounting.Query
{
    public interface IDocumentAttachmentQuery : IQuery<DocumentAttachment, short>
    {
        IList<DocumentAttachment> FindByActive();
        IList<DocumentAttachment> GetDocumentAttachmentByDocumentID(long documentID);
    }
}
