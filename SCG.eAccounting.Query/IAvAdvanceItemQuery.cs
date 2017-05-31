using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;

using SS.Standard.Data.Query;
using SS.Standard.Data.Query.NHibernate;

namespace SCG.eAccounting.Query
{
	public interface IAvAdvanceItemQuery : IQuery<AvAdvanceItem, long>
	{
        IList<AvAdvanceItem> FindByAvAdvanceItemAdvanceID(long advanceId);
        AvAdvanceItem GetByAvAdvanceItemAdvanceID(long advanceId);

		InvoiceExchangeRate GetAdvanceExchangeRate(IList<long> advanceIDlist, short currencyID);
	}
}
