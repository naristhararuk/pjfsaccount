using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;

using SS.Standard.Data.NHibernate.Service;
using SS.Standard.Data.NHibernate.Query;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;

namespace SS.SU.Query
{
	public interface ISuPasswordHistoryQuery : IQuery<SuPasswordHistory, long>
	{
		IList<SuPasswordHistory> FindActiveByUserId(long userId);
	}
}
