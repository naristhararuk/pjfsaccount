using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.SU.DTO;
using SS.SU.DAL;
using NHibernate;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SS.SU.DAL.Hibernate
{
    public partial class SuRoleServiceDao : NHibernateDaoBase<SS.SU.DTO.SuRoleService,long>,ISuRoleServiceDao
    {

        #region ISuRoleServiceDao Members

        public bool IsExist(SS.SU.DTO.SuRoleService roleService)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            sqlBuilder.Append("SELECT COUNT(1) AS RoleServiceCount ");
            sqlBuilder.Append("FROM suroleservice ");
            sqlBuilder.Append("WHERE roleid=:roleID ");
            sqlBuilder.Append("AND serviceteamid=:serviceTeamid");
            parameterBuilder.AddParameterData("roleID", typeof(short), roleService.RoleID.RoleID);
            parameterBuilder.AddParameterData("serviceTeamid", typeof(long), roleService.ServiceTeamID.ServiceTeamID);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);

            query.AddScalar("RoleServiceCount", NHibernateUtil.Int32);
            int count = query.UniqueResult<int>();
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
