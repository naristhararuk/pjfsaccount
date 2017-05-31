using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;

namespace SS.SU.DAL
{
    public interface ISuUserRoleDao : IDao<SuUserRole, long>
    {
		void DeleteByRoleIdAndUserId(short roleId, long userId);
    }
}
