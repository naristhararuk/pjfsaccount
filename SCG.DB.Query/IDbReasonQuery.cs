using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SCG.DB.DTO;
using SS.Standard.Data.Query;
using SS.Standard.Data.Query.NHibernate;
using NHibernate;
using SCG.DB.DTO.ValueObject;

namespace SCG.DB.Query
{
    public interface IDbReasonQuery : IQuery<DbScgReason, int>
    {
        ISQLQuery FindReasonByDocumentTypeLanguageId(string documentType, short languageId, bool isCount, string sortExpression);
        IList<DbScgReason> GetReasonList(string documentType, short languageId, int firstResult, int maxResult, string sortExpression);
        int GetReasonCount(string documentType, short languageId);
        IList<VORejectReasonLang> FindRejectReasonByDocumentTypeIDAndLanguageID(int documentTypeID, short languageID);
    }
}
