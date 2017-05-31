using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.Query;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SCG.DB.DTO;
using SCG.DB.Query;
using SCG.DB.DTO.ValueObject;
using NHibernate.Expression;
using System.Collections;
using SS.Standard.Utilities;
using SS.DB.DTO.ValueObject;


namespace SCG.DB.Query.Hibernate
{
    public class DbServiceTeamQuery : NHibernateQueryBase<DbServiceTeam, long>, IDbServiceTeamQuery
    {
        public ISQLQuery FindServiceTeamByCriteria(DbServiceTeam serviceTeam, bool isCount, string sortExpression)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            if (!isCount)
            {
                sqlBuilder.Append("SELECT c.ServiceTeamID AS ServiceTeamID, c.ServiceTeamCode AS ServiceTeamCode, c.Description AS Description, c.Active AS Active ");
                sqlBuilder.Append("FROM DbServiceTeam c ");
                sqlBuilder.Append("WHERE c.ServiceTeamCode Like :ServiceTeamCode ");
                sqlBuilder.Append("AND c.Description Like :Description ");

                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine("ORDER BY c.ServiceTeamCode,c.Description,c.Active ");
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));

            }
            else
            {
                sqlBuilder.Append("SELECT COUNT(ServiceTeamID) AS ServiceTeamCount ");
                sqlBuilder.Append("FROM DbServiceTeam c ");
                sqlBuilder.Append("WHERE c.ServiceTeamCode Like :ServiceTeamCode ");
                sqlBuilder.Append("AND c.Description Like :Description ");
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("ServiceTeamCode", typeof(string), string.Format("%{0}%", serviceTeam.ServiceTeamCode));
            queryParameterBuilder.AddParameterData("Description", typeof(string), string.Format("%{0}%", serviceTeam.Description));
            queryParameterBuilder.FillParameters(query);

            if (!isCount)
            {
                query.AddScalar("ServiceTeamID", NHibernateUtil.Int64);
                query.AddScalar("ServiceTeamCode", NHibernateUtil.String);
                query.AddScalar("Description", NHibernateUtil.String);
                query.AddScalar("Active", NHibernateUtil.Boolean);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(DbServiceTeam)));
            }
            else
            {
                query.AddScalar("ServiceTeamCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }
            return query;
        }

        public int CountServiceTeamByCriteria(DbServiceTeam serviceTeam)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbServiceTeamQuery, "FindServiceTeamByCriteria", new object[] { serviceTeam, true, string.Empty });
        }

        public IList<DbServiceTeam> GetServiceTeamList(DbServiceTeam serviceTeam, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<DbServiceTeam>(ScgDbQueryProvider.DbServiceTeamQuery, "FindServiceTeamByCriteria", new object[] { serviceTeam, false, sortExpression }, firstResult, maxResult, sortExpression);
        }

        #region IDbServiceTeamQuery Members

        /// <summary>
        /// use for provide data to drop down list
        /// </summary>
        /// <returns></returns>
        public IList<TranslatedListItem> GetAllServiceTeamListItem()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            sqlBuilder.Append("SELECT dst.serviceteamid AS strID,");
            sqlBuilder.Append("'[ ' + dst.serviceteamcode + ' ] ' + dst.description AS strSymbol ");
            sqlBuilder.Append("FROM dbserviceteam dst ");
            sqlBuilder.Append("WHERE dst.active=1 ");
            //sqlBuilder.Append("ORDER BY dst.serviceteamcode");
            sqlBuilder.Append("ORDER BY strSymbol");
            
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);
            query.AddScalar("strID", NHibernateUtil.String)
                .AddScalar("strSymbol", NHibernateUtil.String);
            return query.SetResultTransformer(Transformers.AliasToBean(typeof(TranslatedListItem))).List<TranslatedListItem>();
        }

        #endregion

        public IList<TranslatedListItem> GetServiceTeamListItemByUserID(long userID, IList<short> userRole)
        {
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.AppendLine(" SELECT dst.serviceteamid AS strID,");
            sqlBuilder.AppendLine(" '[ ' + dst.serviceteamcode + ' ] ' + dst.description AS strSymbol ");
            sqlBuilder.AppendLine(" FROM DbServiceTeam dst ");

            if (userID != 0)
            {
                sqlBuilder.AppendLine(" INNER JOIN SuRoleService srs ");
                sqlBuilder.AppendLine(" ON dst.ServiceTeamID = srs.ServiceTeamID ");
                sqlBuilder.AppendLine(" INNER JOIN SuUserRole sur ");
                sqlBuilder.AppendLine(" ON sur.RoleID = srs.RoleID ");
                sqlBuilder.AppendLine(" INNER JOIN SuUser su ");
                sqlBuilder.AppendLine(" ON su.UserID = sur.UserID ");
                sqlBuilder.AppendLine(" INNER JOIN SuRole sr ");
                sqlBuilder.AppendLine(" ON sr.RoleID = sur.RoleID ");
            }

            sqlBuilder.AppendLine(" WHERE dst.Active = 1");

            if (userID != 0)
            {
                sqlBuilder.AppendLine(" AND su.UserID = :UserID ");
                sqlBuilder.AppendLine(" AND sr.RoleID in (:UserRoleList) ");
                sqlBuilder.AppendLine(" GROUP BY dst.ServiceTeamID,dst.ServiceTeamCode,dst.Description ");
            }

            //sqlBuilder.AppendLine(" ORDER BY strID");
            sqlBuilder.AppendLine(" ORDER BY strSymbol");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            if (userID != 0)
            {
                query.SetInt64("UserID", userID);
                query.SetParameterList("UserRoleList", userRole);
            }

            query.AddScalar("strID", NHibernateUtil.String);
            query.AddScalar("strSymbol", NHibernateUtil.String);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(TranslatedListItem))).List<TranslatedListItem>();
        }

        public IList<TranslatedListItem> GetServiceTeamListItemByUserID(short RoleID)
        {
            //StringBuilder sqlBuilder = new StringBuilder();

            //sqlBuilder.AppendLine(" SELECT dst.serviceteamid AS strID,");
            //sqlBuilder.AppendLine(" '[ ' + dst.serviceteamcode + ' ] ' + dst.description AS strSymbol ");
            //sqlBuilder.AppendLine(" FROM DbServiceTeam dst ");

            //if (userID != 0)
            //{
            //    sqlBuilder.AppendLine(" INNER JOIN SuRoleService srs ");
            //    sqlBuilder.AppendLine(" ON dst.ServiceTeamID = srs.ServiceTeamID ");
            //    sqlBuilder.AppendLine(" INNER JOIN SuUserRole sur ");
            //    sqlBuilder.AppendLine(" ON sur.RoleID = srs.RoleID ");
            //    sqlBuilder.AppendLine(" INNER JOIN SuUser su ");
            //    sqlBuilder.AppendLine(" ON su.UserID = sur.UserID ");
            //    sqlBuilder.AppendLine(" INNER JOIN SuRole sr ");
            //    sqlBuilder.AppendLine(" ON sr.RoleID = sur.RoleID ");
            //}

            //sqlBuilder.AppendLine(" WHERE dst.Active = 1");

            //if (userID != 0)
            //{
            //    sqlBuilder.AppendLine(" AND su.UserID = :UserID ");
            //    sqlBuilder.AppendLine(" AND sr.RoleID in (:UserRoleList) ");
            //    sqlBuilder.AppendLine(" GROUP BY dst.ServiceTeamID,dst.ServiceTeamCode,dst.Description ");
            //}

            //sqlBuilder.AppendLine(" ORDER BY strID");

            //ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            //if (userID != 0)
            //{
            //    query.SetInt64("UserID", userID);
            //    query.SetParameterList("UserRoleList", userRole);
            //}

            //query.AddScalar("strID", NHibernateUtil.String);
            //query.AddScalar("strSymbol", NHibernateUtil.String);

            //return query.SetResultTransformer(Transformers.AliasToBean(typeof(TranslatedListItem))).List<TranslatedListItem>();

            //-----
            StringBuilder sqlbuilder = new StringBuilder();

            sqlbuilder.AppendLine(" SELECT dst.serviceteamid AS strID,");
            sqlbuilder.AppendLine(" '[ ' + dst.serviceteamcode + ' ] ' + dst.description AS strSymbol ");
            sqlbuilder.AppendLine(" FROM DbServiceTeam dst ");
            sqlbuilder.Append(" where 1=1 ");
            sqlbuilder.Append(" and dst.active = 1 ");

            if (RoleID != 0)
                sqlbuilder.Append("and dst.ServiceTeamID not in (select ServiceTeamID from SuRoleService WHERE roleid = ").Append(RoleID.ToString()).Append(")");

            //sqlbuilder.AppendLine(" ORDER BY strID");
            sqlbuilder.AppendLine(" ORDER BY strSymbol");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlbuilder.ToString());
            query.AddScalar("strID", NHibernateUtil.String)
                .AddScalar("strSymbol", NHibernateUtil.String);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(TranslatedListItem))).List<TranslatedListItem>();
        }

        public IList<TranslatedListItem> GetAllServiceTeamListItemByCompanyId(long? companyId)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            sqlBuilder.Append("SELECT dst.ServiceteamId AS strID,");
            sqlBuilder.Append("'[ ' + dst.ServiceteamCode + ' ] ' + dst.Description AS strSymbol ");
            sqlBuilder.Append("FROM DbServiceTeam dst ");
            
            string whereClause = " WHERE dst.active=1 ";
            if (companyId.HasValue && companyId.Value > 0)
            {
                sqlBuilder.Append(" INNER JOIN DbServiceTeamLocation sl on dst.ServiceTeamID = sl.ServiceTeamID ");
                sqlBuilder.Append(" INNER JOIN DbLocation l on l.LocationID = sl.LocationID ");
                whereClause += " and l.CompanyID = :CompanyID";
                parameterBuilder.AddParameterData("CompanyID", typeof(long), companyId.Value);
    }
            sqlBuilder.Append(whereClause.ToString());
            sqlBuilder.Append(" GROUP BY dst.ServiceTeamID, dst.ServiceTeamCode, dst.Description ");
            sqlBuilder.Append(" ORDER BY strSymbol");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);
            query.AddScalar("strID", NHibernateUtil.String)
                .AddScalar("strSymbol", NHibernateUtil.String);
            return query.SetResultTransformer(Transformers.AliasToBean(typeof(TranslatedListItem))).List<TranslatedListItem>();
}
    }
}
