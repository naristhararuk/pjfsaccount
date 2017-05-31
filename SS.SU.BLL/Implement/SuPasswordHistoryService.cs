using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SS.SU.DTO;
using SS.SU.BLL;
using SS.SU.DAL;

namespace SS.SU.BLL.Implement
{
	public class SuPasswordHistoryService : ServiceBase<SuPasswordHistory, long>, ISuPasswordHistoryService
	{
		#region Override Method
		public override IDao<SuPasswordHistory, long> GetBaseDao()
		{
			return DaoProvider.SuPasswordHistoryDao;
		}
		#endregion

		#region ISuPasswordHistoryService Members
		public IList<SuPasswordHistory> FindPasswordHistoryByUserId(long userId)
		{
			throw new NotImplementedException();
		}
		public IList<SuPasswordHistory> FindActivePasswordHistoryByUserId(long userId)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}
