using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;

using SS.Standard.Data.NHibernate.Query;
using SS.Standard.Data.NHibernate.Service;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;

namespace SS.SU.Query
{
    public interface ISuRTEContentQuery : IQuery<SuRTEContent, short>
    {
        IList<SuRTEContentSearchResult> FindSuRTEContentByNodeId(short nodeId);
        IList<SuRTEContentSearchResult> FindSuRTEContentByContentIdLanguageId(short contentId, short languageId);
    }
}
