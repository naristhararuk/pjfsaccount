using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;

using NHibernate;

namespace SS.SU.DAL
{
    public interface ISuRTENodeDao : IDao<SuRTENode, short>
    {
        IList<SuRTENodeSearchResult> GetRTENodeList(short languageId, string nodetype);
        IList<SuRTENodeSearchResult> GetRTEContentList(short languageId, string nodetype,short nodeId);
        IList<SuRTENodeSearchResult> GetRTEContent(short languageId, string nodetype, short nodeId);
        SuRTENodeSearchResult GetWelcome(short languageId, string nodeType);
    }
}
