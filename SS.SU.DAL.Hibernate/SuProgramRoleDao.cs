using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using SS.SU.DAL;
using NHibernate.Expression;
using SS.SU.DTO.ValueObject;
using NHibernate;
using SS.Standard.Data.NHibernate.QueryCreator;
using NHibernate.Transform;


namespace SS.SU.DAL.Hibernate
{
    public partial class SuProgramRoleDao : NHibernateDaoBase<SuProgramRole, short>, ISuProgramRoleDao
    {
        public SuProgramRoleDao()
        {

        }
        /// <summary>
        /// Check via RoleID and ProgramID, Is it aready exist in table.
        /// </summary>
        /// <param name="programeRole">programRole DTO contained : roleID and ProgramID propeties.</param>
        /// <returns></returns>
        public bool IsExist(SuProgramRole programeRole)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder paramBuilder = new QueryParameterBuilder();
            sqlBuilder.Append("SELECT COUNT(1) AS ProgramCount ");
            sqlBuilder.Append("FROM suprogramrole ");
            sqlBuilder.Append("WHERE roleid = :roleID ");
            sqlBuilder.Append("AND programid = :programID ");
            sqlBuilder.Append("AND active = 1 ");
            paramBuilder.AddParameterData("roleID", typeof(short), programeRole.Role.RoleID);
            paramBuilder.AddParameterData("programID", typeof(long), programeRole.Program.Programid);
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            paramBuilder.FillParameters(query);
            query.AddScalar("ProgramCount", NHibernateUtil.Int32);
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
        public void DeleteByProgramRoleId(short roleId, short programId)
        {
            GetCurrentSession()
                .Delete("from SuProgramRole pr where pr.Role.Roleid = :roleId and pr.Program.Programid = :programId"
                , new object[] { roleId, programId }
                , new NHibernate.Type.IType[] { NHibernateUtil.Int16, NHibernateUtil.Int16 });
        }
    }
}
