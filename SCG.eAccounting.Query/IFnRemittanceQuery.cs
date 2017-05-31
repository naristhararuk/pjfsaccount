using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SCG.eAccounting.DTO;

using SS.Standard.Data.Query;
using SS.Standard.Data.Query.NHibernate;

namespace SCG.eAccounting.Query
{
    public interface IFnRemittanceQuery : IQuery<FnRemittance, long>
	{
        FnRemittance GetFnRemittanceByDocumentID(long documentID);
        IList<FnRemittance> FindRemittanceReferenceTAByTADocumentID(long taDocumentID);
        IList<FnRemittance> FindRemittanceReferenceTAForRequesterByTADocumentID(long requesterID, long taDocumentID);
	}
}
