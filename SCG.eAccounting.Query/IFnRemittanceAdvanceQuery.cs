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
    public interface IFnRemittanceAdvanceQuery : IQuery<FnRemittanceAdvance, long>
	{
        IList<FnRemittanceAdvance> FindRemittanceAdvanceByRemittanceID(long remittanceID);
        FnRemittanceAdvance FindRemittanceAdvanceByRemittanceIDAndAdvanceID(long remittanceID, long advanceID);
        IList<Advance> FindRemittanceAdvanceAndItemsByAdvanceIDs(List<long> advanceIdList);
        IList<AdvanceData> FindByRemittanceIDForUpdateRemittanceAdvance(long remittanceID);
        IList<FnRemittanceAdvance> FindRemittanceReferenceAdvanceByAdvanceID(long advanceID);
	}
}