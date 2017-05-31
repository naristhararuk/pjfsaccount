using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;

using SS.Standard.Data.Query;
using SS.Standard.Data.Query.NHibernate;
using NHibernate;

namespace SCG.eAccounting.Query
{
    public interface IFnEACAdvanceImportLogQuery : IQuery<FnEACAdvanceImportLog, long>
    {
        ISQLQuery FindFnEACAdvanceImportLogSearchResult(FnEACAdvanceImportLog fnEACAdvanceImportLog, string sortExpression, bool isCount);
        IList<FnEACAdvanceImportLog> FindFnEACAdvanceImportLogListQuery(FnEACAdvanceImportLog fnEACAdvanceImportLog, int firstResult, int maxResult, string sortExpression);
        int GetCountFnEACAdvanceImportLoglist(FnEACAdvanceImportLog fnEACAdvanceImportLog);
    }
}
