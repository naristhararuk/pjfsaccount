using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using SS.Standard.Data.NHibernate.Service;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;

namespace SS.SU.BLL
{
    public interface ISuRTENodeService : IService<SuRTENode, short>
    {
        IList<SuRTENodeSearchResult> GetRTENodeList(short languageId, string nodetype);
        IList<SuRTENodeSearchResult> GetRTEContentList(short languageId, string nodetype,short nodeid);
        IList<SuRTENodeSearchResult> GetRTEContent(short languageId, string nodetype, short nodeid);
        SuRTENodeSearchResult GetWelcome(short languageId, string nodeType);
        short AddNode(SuRTENode node);
        short AddNode(SuRTENode node, HttpPostedFile imageFile);
        void UpdateNode(SuRTENode node);
        void UpdateNode(SuRTENode node, HttpPostedFile imageFile);
    }
}
