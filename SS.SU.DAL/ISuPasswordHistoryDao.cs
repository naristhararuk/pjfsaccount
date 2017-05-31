using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.SU.DTO;

using SS.Standard.Data.NHibernate.Dao;

namespace SS.SU.DAL
{
	public interface ISuPasswordHistoryDao : IDao<SuPasswordHistory, long>
	{
		IList<SuPasswordHistory> FindPasswordHistoryByUserId(long userId);
		IList<SuPasswordHistory> FindActivePasswordHistoryByUserId(long userId);
	}
}