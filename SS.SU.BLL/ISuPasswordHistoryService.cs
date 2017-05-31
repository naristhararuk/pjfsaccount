using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SS.SU.DTO;

namespace SS.SU.BLL
{
	public interface ISuPasswordHistoryService : IService<SuPasswordHistory, long>
	{
		IList<SuPasswordHistory> FindPasswordHistoryByUserId(long userId);
		IList<SuPasswordHistory> FindActivePasswordHistoryByUserId(long userId);
	}
}
