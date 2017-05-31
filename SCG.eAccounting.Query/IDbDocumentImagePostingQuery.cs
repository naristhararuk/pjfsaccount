using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.Query.NHibernate;
using SCG.eAccounting.DTO;

namespace SCG.eAccounting.Query
{
    public interface IDbDocumentImagePostingQuery : IQuery<DbDocumentImagePosting,long>
    {

        IList<DbDocumentImagePosting> GetDocumentImagePostingByStatusCode(string status);
        DbDocumentImagePosting GetDocumentImagePostingByImageID(string ImageID);
        long GetWorkflowIdByDocumentID(long documentID);
    }
}
