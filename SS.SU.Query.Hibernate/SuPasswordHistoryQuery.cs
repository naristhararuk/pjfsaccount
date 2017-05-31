using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.Query;

using SS.SU.DTO;
using SS.SU.Query;

namespace SS.SU.Query.Hibernate
{
	public class SuPasswordHistoryQuery : NHibernateQueryBase<SuPasswordHistory, long>, ISuPasswordHistoryQuery
	{
		#region ISuPasswordHistoryQuery Members
		public IList<SuPasswordHistory> FindActiveByUserId(long userId)
		{
			StringBuilder sqlBuilder = new StringBuilder();
			sqlBuilder.AppendLine(" FROM SuPasswordHistory sph WHERE sph.User.Userid = :userId AND sph.Active = 'true' ");
			
			return GetCurrentSession().CreateQuery(sqlBuilder.ToString()).SetInt64("userId", userId).List<SuPasswordHistory>();
		}
		#endregion
	}
}
