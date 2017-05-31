using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.Query.NHibernate;
using SCG.eAccounting.DTO;

namespace SCG.eAccounting.Query
{
    public interface ITADocumentTravellerQuery : IQuery<TADocumentTraveller, int>
    {
        IList<TADocumentTraveller> FindTADocumentTravellerByTADocumentID(long taDocumentID);
        IList<TADocumentTraveller> FindTADocumentTravellerByTADocumentIDAndUserID(long taDocumentID, long UserID);
    }
}
