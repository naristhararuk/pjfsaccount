using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.Query.NHibernate;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;
using SS.SU.DTO;

namespace SCG.eAccounting.Query
{
    public interface ITADocumentAdvanceQuery : IQuery<TADocumentAdvance, int>
    {
        IList<TADocumentAdvance> FindTADocumentAdvanceByTADocumentID(long taDocumentID);
        IList<TADocumentObj> FindAdvanceByTADocumentID(short languageID, long taDocumentID);
        IList<SuUser> FindUserIDAdvanceByTADocumentID(long taDocumentID);
    }
}
