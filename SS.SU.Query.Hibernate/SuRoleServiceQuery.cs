using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using SS.Standard.Data.NHibernate.QueryCreator;
using NHibernate;
using NHibernate.Transform;

namespace SS.SU.Query.Hibernate
{
    public class SuRoleServiceQuery : NHibernateQueryBase<SuRoleService,long>, ISuRoleServiceQuery
    {

        #region ISuRoleServiceQuery Members

        public IList<ServiceTeamInformation> GetServiceTeamInformation(short roleID,string sortExpression)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            sqlBuilder.Append("SELECT srs.roleserviceid AS RoleServiceID,");
            sqlBuilder.Append("dst.serviceteamcode AS ServiceCode,");
            sqlBuilder.Append("dst.description AS ServiceName ");
            sqlBuilder.Append("FROM suroleservice srs,dbserviceteam dst ");
            sqlBuilder.Append("WHERE srs.active=1 ");
            sqlBuilder.Append("AND srs.roleid = :roleID ");
            sqlBuilder.Append("AND srs.serviceteamid = dst.serviceteamid ");
            if (string.IsNullOrEmpty(sortExpression))
            {
                sqlBuilder.Append(" ORDER BY RoleServiceID, ServiceCode, ServiceName");
            }
            else
            {
                sqlBuilder.Append(" ORDER BY " + sortExpression);
            }

            parameterBuilder.AddParameterData("roleID", typeof(Int16), roleID);
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);
            query.AddScalar("RoleServiceID",NHibernateUtil.Int64);
            query.AddScalar("ServiceCode",NHibernateUtil.String);
            query.AddScalar("ServiceName", NHibernateUtil.String);

            query.SetResultTransformer(Transformers.AliasToBean(typeof(ServiceTeamInformation)));
            return query.List<ServiceTeamInformation>();
        }

   

        public IList<SuRole> GetRoleByServiceTeamID(long serviceTeamID)
        {
            IList<SuRole> role = new List<SuRole>();

            IList<SuRoleService> roleServices = GetCurrentSession().CreateQuery("from SuRoleService where ServiceTeamID = :ServiceTeamID and active = '1'")
                .SetInt64("ServiceTeamID", serviceTeamID)
                .List<SuRoleService>();

            foreach (SuRoleService roleService in roleServices)
            {
                role.Add(roleService.RoleID);
            }

            return role;
        }

        #endregion
    }
}
