using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;
using SCG.FN.DTO;
using SCG.FN.DTO.ValueObject;
using System.Data;

namespace SCG.FN.BLL
{
    public interface IFnExpenseDocumentService : IService<FnExpenseDocument, long>
    {
        DataSet PrepareExpenseDS();
        DataSet PrepareExpenseDS(long documentId);
        Guid BeginExpenseTransaction();
        long AddNewExpenseDocumentOnTransaction(FnExpenseDocument exp, Guid txId);
        void UpdateExpenseDocumentOnTransaction(FnExpenseDocument exp, Guid txId);
        long AddExpenseDocument(Guid txId, long expDocumentId);
    }
}
