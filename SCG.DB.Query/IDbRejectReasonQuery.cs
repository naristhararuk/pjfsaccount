using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;
using SS.Standard.Data.Query;
using SS.Standard.Data.Query.NHibernate;

using NHibernate;

using SCG.DB.DTO;
using SCG.DB.DTO.ValueObject;

namespace SCG.DB.Query
{
    public interface IDbRejectReasonQuery : IQuery<DbRejectReason, int>
    {
        IList<VORejectReasonLang> FindRejectReasonByDocTypeIDStateIDAndLanguageID(int documentTypeID, int WorkflowStateID, short languageID);

        IList<VORejectReasonLang> FindRejectReasonByDocTypeIDStateEventIDAndLanguageID(int documentTypeID, int WorkflowStateID, short languageID);
    }
}
