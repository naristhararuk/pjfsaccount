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
    public interface IRejectReasonQuery : IQuery<DbRejectReason, int>
    {
        ISQLQuery FindReasonByDocumentTypeLanguageId(DbRejectReason dbRejectReason, short languageId, bool isCount, string sortExpression);
        IList<VORejectReasonLang> GetRejectReasonList(DbRejectReason dbRejectReason, short languageId, int firstResult, int maxResult, string sortExpression);
        int GetRejectReasonCount(DbRejectReason dbRejectReason, short languageId);
        DbRejectReason FindByCode(string code);
    }
}
