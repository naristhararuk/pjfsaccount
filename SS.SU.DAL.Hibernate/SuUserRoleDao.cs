using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using SS.SU.DAL;

namespace SS.SU.DAL.Hibernate
{
    public partial class SuUserRoleDao : NHibernateDaoBase<SuUserRole, long>, ISuUserRoleDao
    {
		#region ISuUserRoleDao Members
		public void DeleteByRoleIdAndUserId(short roleId, long userId)
		{
			GetCurrentSession().Delete("FROM SuUserRole ur WHERE ur.Role.Roleid = :roleId and ur.User.Userid = :userId"
				, new object[] { roleId, userId }
				, new NHibernate.Type.IType[] { NHibernateUtil.Int16, NHibernateUtil.Int64 });
		}
		#endregion


	}
}
