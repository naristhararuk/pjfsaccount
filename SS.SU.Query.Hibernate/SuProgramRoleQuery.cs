using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.QueryDao;

using SS.SU.DTO;
using SS.SU.Query;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.QueryCreator;
using NHibernate;
using NHibernate.Transform;
using SS.SU.DTO.ValueObject;
using NHibernate.Expression;
using System.Collections;

namespace SS.SU.Query.Hibernate
{
    public class SuProgramRoleQuery : NHibernateQueryBase<SuProgramRole, short>, ISuProgramRoleQuery
    {
        public IList<SuProgramRole> FindAllByLoopObject()
        {
            IList<SuProgramRole> suProgramRole = FindAll();
            foreach (SuProgramRole sr in suProgramRole)
            {
                bool status = sr.Active;
                // Console.Write(sr.Role);
            }
            return suProgramRole;
        }

        public IList<ProgramRole> FindSuProgramRoleByRoleId(short RoleId, short LanguageId)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT SuProgramRole.ID,SuProgramRole.RoleId,SuProgramRole.ProgramId,SuProgramLang.LanguageId,SuProgramLang.ProgramsName");
            sqlBuilder.Append(" ,AddState,EditState,DeleteState,DisplayState,SuProgramRole.Comment,SuProgramRole.Active");
            sqlBuilder.Append(" FROM SuProgramRole LEFT OUTER JOIN SuProgramLang ON SuProgramLang.ProgramID = SuProgramRole.ProgramID");
            sqlBuilder.Append(" AND SuProgramLang.LanguageID = :LanguageId");
            sqlBuilder.Append(" WHERE SuProgramRole.RoleID = :RoleId");
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("RoleId", typeof(short), RoleId);
            parameterBuilder.AddParameterData("LanguageId", typeof(short), LanguageId);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);
            query.AddScalar("ID", NHibernateUtil.Int16)
                .AddScalar("RoleId", NHibernateUtil.Int16)
                .AddScalar("ProgramId", NHibernateUtil.Int16)
                .AddScalar("ProgramsName", NHibernateUtil.String)
                .AddScalar("LanguageId", NHibernateUtil.Int16)
                .AddScalar("AddState", NHibernateUtil.Boolean)
                .AddScalar("EditState", NHibernateUtil.Boolean)
                .AddScalar("DeleteState", NHibernateUtil.Boolean)
                .AddScalar("DisplayState", NHibernateUtil.Boolean)
                .AddScalar("Comment", NHibernateUtil.String)
                .AddScalar("Active", NHibernateUtil.Boolean);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(SU.DTO.ValueObject.ProgramRole))).List<ProgramRole>();
        }

        public ISQLQuery FindBySuProgramRoleQuery(SuProgramRole programRole, short languageId, string sortExpression, bool isCount)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            StringBuilder orderBy = new StringBuilder();
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();

            if (isCount)
            {
                sqlBuilder.Append(" select SELECT count(*) as Count ");
                sqlBuilder.Append(" FROM SuProgramRole pr LEFT OUTER JOIN SuProgramLang pl ON pl.ProgramID = pr.ProgramID");
            }
            else
            {
                sqlBuilder.Append(" select pr.ID , pr.RoleID , pr.ProgramID, pl.LanguageId , pl.ProgramsName ,  pr.AddState , pr.EditState , pr.DeleteState , pr.DisplayState , pr.Comment ,pr.Active ");
                sqlBuilder.Append(" FROM SuProgramRole pr LEFT OUTER JOIN SuProgramLang pl ON pl.ProgramID = pr.ProgramID");
            }
            StringBuilder whereClauseBuilder = new StringBuilder();
            string whereClause = "1=1";

            whereClauseBuilder.Append(" and pl.Languageid = :LanguageId");
            queryParameterBuilder.AddParameterData("LanguageId", typeof(short), languageId);

            whereClauseBuilder.Append(" and pr.RoleID = :RoleId");
            queryParameterBuilder.AddParameterData("RoleId", typeof(short), programRole.Role.RoleID);

            if (string.IsNullOrEmpty(sortExpression))
            {
                orderBy.AppendLine(" ORDER BY pr.ID , pr.RoleID , pr.ProgramID, pl.LanguageId , pl.ProgramsName ,  pr.AddState , pr.EditState , pr.DeleteState , pr.DisplayState , pr.Comment ,pr.Active ");
            }
            else
            {
                orderBy.AppendLine(string.Format(" ORDER BY {0}", sortExpression));
            }

            whereClause += whereClauseBuilder.ToString();

            if (!string.IsNullOrEmpty(whereClause))
            {
                sqlBuilder.Append(" where " + whereClause + orderBy);
            }

            string sql = sqlBuilder.ToString();

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql);

            queryParameterBuilder.FillParameters(query);
            query.AddScalar("ID", NHibernateUtil.Int16);
            query.AddScalar("RoleId", NHibernateUtil.Int16);
            query.AddScalar("ProgramId", NHibernateUtil.Int16);
            query.AddScalar("LanguageId", NHibernateUtil.Int16);
            query.AddScalar("ProgramsName", NHibernateUtil.String);
            query.AddScalar("AddState", NHibernateUtil.Boolean);
            query.AddScalar("EditState", NHibernateUtil.Boolean);
            query.AddScalar("DeleteState", NHibernateUtil.Boolean);
            query.AddScalar("DisplayState", NHibernateUtil.Boolean);
            query.AddScalar("Comment", NHibernateUtil.String);
            query.AddScalar("Active", NHibernateUtil.Boolean);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(ProgramRole)));
            return query;
        }

        public IList<ProgramRole> FindBySuProgramRole(SuProgramRole programRole, short languageId, int firstResult, int maxResults, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<ProgramRole>(QueryProvider.SuProgramRoleQuery, "FindBySuProgramRoleQuery", new object[] { programRole, languageId, sortExpression, false }, firstResult, maxResults, sortExpression);
        }

        public int CountBySuProgramRoleCriteria(SuProgramRole criteria, short languageId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select count(*) as count");
            sql.Append(" FROM SuProgramRole pr LEFT OUTER JOIN SuProgramLang pl ON pl.ProgramID = pr.ProgramID");
            sql.Append(" where pl.Languageid = :LanguageId");
            sql.Append(" and pr.RoleID = :RoleId");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.SetInt16("RoleId", criteria.Role.RoleID);
            query.SetInt16("LanguageId", languageId);
            query.AddScalar("count", NHibernateUtil.Int32);

            return Convert.ToInt32(query.UniqueResult());
        }
        public IList<SuProgramRole> FindProgramPermission(ArrayList arrayRoleID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("select * from SuProgramRole where roleID ");
            sqlBuilder.Append(" in (");
            string strValue = "";
            for (int i = 0; i < arrayRoleID.Count; i++)
            {
                strValue += "," + arrayRoleID[i].ToString();
            }
            sqlBuilder.AppendFormat("{0}", strValue.TrimStart(','));
            sqlBuilder.Append(")");

            ISQLQuery queryTransactionPermission = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            queryTransactionPermission.AddEntity(typeof(SuProgramRole));
            IList<SuProgramRole> UserTransactionPermissionList = queryTransactionPermission.List<SuProgramRole>();
            foreach (SuProgramRole programRole in UserTransactionPermissionList)
            {
                Console.Write(programRole.Program);
            }
            return UserTransactionPermissionList;
        }



        public IList<ProgramInformation> FindProgramInfoByRole(SuRole role, short languageID, string sortExpression)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT spr.id AS ProgramRoleID,");
            sqlBuilder.Append("sp.programCode AS ProgramCode,");
            sqlBuilder.Append("spl.programsName AS ProgramsName ");
            sqlBuilder.Append("FROM suprogramrole spr,");
            sqlBuilder.Append("suprogramlang spl,");
            sqlBuilder.Append("suprogram sp ");
            sqlBuilder.Append("WHERE 1=1 ");
            sqlBuilder.Append("AND spr.roleid=:roleID ");
            sqlBuilder.Append("AND spr.programid = sp.programid ");
            sqlBuilder.Append("AND spl.programid = sp.programid ");
            sqlBuilder.Append("AND spl.languageid = :languageID ");
            sqlBuilder.Append("AND spr.active=1 ");
            if (!string.IsNullOrEmpty(sortExpression))
            {
                sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));
            }
            else
            {
                sqlBuilder.Append(" ORDER BY ProgramRoleID,ProgramCode,ProgramsName ");
            }

            QueryParameterBuilder paramBuilder = new QueryParameterBuilder();
            paramBuilder.AddParameterData("roleID", typeof(short), role.RoleID);
            paramBuilder.AddParameterData("languageID", typeof(short), languageID);
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            paramBuilder.FillParameters(query);

            query.AddScalar("ProgramRoleID", NHibernateUtil.Int16)
                .AddScalar("ProgramCode", NHibernateUtil.String)
                .AddScalar("ProgramsName", NHibernateUtil.String);

            query.SetResultTransformer(Transformers.AliasToBean(typeof(ProgramInformation)));
            return query.List<ProgramInformation>();


        }

        public IList<SuProgramRole> FindByProgramCode_UserID(string programCode, long userID)
        {
            ISQLQuery query =  GetCurrentSession().CreateSQLQuery(@"
                select SuProgramRole.*
                from SuProgramRole 
                    inner join  SuUserRole 
                        on SuProgramRole.RoleID = SuUserRole.RoleID
                    inner join SuProgram
                        on SuProgramRole.ProgramID = SuProgram.ProgramID
                where SuProgram.ProgramCode = :ProgramCode
                    and SuUserRole.UserID = :UserID
                ");
                    //and SuProgramRole.Active = 1
                    //and SuUserRole.Active = 1

            query.SetString("ProgramCode", programCode);
            query.SetInt64("UserID", userID);
            query.AddEntity(typeof(SuProgramRole));
            return query.List<SuProgramRole>();
        }

        public IList<ProgramRole> FindSuProgramRoleByProgramCode(string programCode)
        {
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.AppendLine(" SELECT SuProgramRole.RoleId ");
            sqlBuilder.AppendLine(" FROM SuProgramRole INNER JOIN SuProgram ON SuProgram.ProgramID = SuProgramRole.ProgramID ");
            sqlBuilder.AppendLine(" WHERE SuProgram.ProgramCode = :ProgramCode ");
            sqlBuilder.AppendLine(" AND SuProgramRole.Active = 1 ");
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();

            parameterBuilder.AddParameterData("ProgramCode", typeof(string), programCode);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);

            query.AddScalar("RoleId", NHibernateUtil.Int16);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(SU.DTO.ValueObject.ProgramRole))).List<ProgramRole>();
        }
    }

}
