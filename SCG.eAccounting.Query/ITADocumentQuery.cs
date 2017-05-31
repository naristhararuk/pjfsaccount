using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.DTO;
using SS.Standard.Data.Query.NHibernate;
using SCG.eAccounting.DTO.ValueObject;

namespace SCG.eAccounting.Query
{
    public interface ITADocumentQuery : IQuery<TADocument, long>
    {
        TADocument GetTADocumentByDocumentID(long documentID);
    }
}
