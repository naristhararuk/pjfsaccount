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
    public interface IFnRemittanceItemQuery : IQuery<FnRemittanceItem, long>
	{
        IList<FnRemittanceItem> FindRemittanceItemByRemittanceID(long remittanceID);
        IList<RemittanceValueObj> FindRemittanceItemByRemittanceIds(IList<long> remittanceIdList);
        IList<RemittanceValueObj> FindRemittanceItemByRemittanceIds(IList<long> remittanceIdList, short languageID);
	}
}
