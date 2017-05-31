using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SCG.eAccounting.DTO;

using SS.Standard.Data.Query;
using SS.Standard.Data.Query.NHibernate;
using SCG.eAccounting.DTO.ValueObject;

namespace SCG.eAccounting.Query
{
    public interface IFnAutoPaymentQuery : IQuery<FnAutoPayment, long>
    {
        FnAutoPayment GetFnAutoPaymentByDocumentID(long documentID);
        IList<ExportClearing> GetExportClearingListByDate(string sapCode);
        long GetDocumentIDByFIDOCID(string FIDOC, string COMCODE, string YEAR);
        FnAutoPayment GetFnAutoPaymentSuccessByDocumentID(long documentID, int status);
    }
}
