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
    public interface ISuRTENodeQuery : IQuery<SuRTENode, short>
    {
        ISQLQuery FindSuRTENodeByLanguageId(short languageId, string sortExpression, bool isCount);
        IList<SuRTENode> GetSuRTENodeList(short languageId, int firstResult, int maxResult, string sortExpression);
        int GetCountList(short languageId);
    }
}
