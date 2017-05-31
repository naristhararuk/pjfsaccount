using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Query;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.DTO;

namespace SCG.eAccounting.Query
{
    public interface IDbDocumentBoxIDPostingQuery : IQuery<DbDocumentBoxidPosting,long>
    {
        IList<ExportBoxID> GetExportBoxIDList(string sapCode);
    }

}
