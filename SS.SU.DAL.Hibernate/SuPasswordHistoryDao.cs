using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DAL;
using SS.SU.DTO;

namespace SS.SU.DAL.Hibernate
{
	public class SuPasswordHistoryDao : NHibernateDaoBase<SuPasswordHistory, long>, ISuPasswordHistoryDao
	{
		#region ISuPasswordHistoryDao Members
		public IList<SuPasswordHistory> FindPasswordHistoryByUserId(long userId)
		{
			StringBuilder sqlBuilder = new StringBuilder();
			sqlBuilder.AppendLine(" FROM SuPasswordHistory sph WHERE sph.User.Userid = :userID ORDER BY sph.ChangeDate DESC ");
			
			return GetCurrentSession().CreateQuery(sqlBuilder.ToString()).SetInt64("userID", userId).List<SuPasswordHistory>();
		}
		public IList<SuPasswordHistory> FindActivePasswordHistoryByUserId(long userId)
		{
			StringBuilder sqlBuilder = new StringBuilder();
			sqlBuilder.AppendLine(" FROM SuPasswordHistory sph WHERE sph.User.Userid = :userID AND sph.Active = 'true' ");

			return GetCurrentSession().CreateQuery(sqlBuilder.ToString()).SetInt64("userID", userId).List<SuPasswordHistory>();
		}
		#endregion
	}
}
