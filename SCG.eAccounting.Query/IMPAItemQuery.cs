using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.Query.NHibernate;
using SCG.eAccounting.DTO;

namespace SCG.eAccounting.Query
{
    public interface IMPAItemQuery : IQuery<MPAItem, long>
    {
        IList<MPAItem> FindMPAItemByMPADocumentID(long MPADocumentID);
    }
}
