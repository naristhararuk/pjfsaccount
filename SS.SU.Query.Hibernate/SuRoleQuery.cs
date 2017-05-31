using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Query;
using SS.SU.DTO;
using SS.SU.Query;
using SS.Standard.Data.NHibernate.QueryDao;
using NHibernate;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.QueryCreator;
using SS.SU.DTO.ValueObject;

namespace SS.SU.Query.Hibernate
{
    public class SuRoleQuery : NHibernateQueryBase<SuRole, short>, ISuRoleQuery
    {
        #region ISuRoleQuery Members

        public ISQLQuery FindByRoleCriteria(SuRole role, string sortExpression, bool isCount, short languageId)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            if (!isCount)
            {
                sqlBuilder.Append("select r.RoleId as RoleID,r.RoleCode as RoleCode,r.Active as Active,r.UpdDate as UpdDate,r.RoleName as RoleName ");
                sqlBuilder.Append("from SuRole as r ");

                sqlBuilder.Append("where 1=1 ");

                if (!string.IsNullOrEmpty(role.RoleCode))
                {
                    sqlBuilder.Append("and r.RoleCode like :roleCode ");
                    parameterBuilder.AddParameterData("roleCode", typeof(string), string.Format("%{0}%", role.RoleCode));

                }
                if (!string.IsNullOrEmpty(role.RoleName))
                {
                    sqlBuilder.Append("and r.RoleName like :roleName ");
                    parameterBuilder.AddParameterData("roleName", typeof(string), string.Format("%{0}%", role.RoleName));
                }

                if (string.IsNullOrEmpty(sortExpression))
                {
                    sqlBuilder.AppendLine("ORDER BY RoleID,UpdDate,RoleName,Active");
                }
                else
                {
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));

                }
            }
            else
            {
                sqlBuilder.Append("select count(r.Roleid) as RoleCount from SuRole r ");
                sqlBuilder.Append("where 1=1 ");
                if (!string.IsNullOrEmpty(role.RoleCode))
                {
                    sqlBuilder.Append("and r.RoleCode like :roleCode ");
                    parameterBuilder.AddParameterData("roleCode", typeof(string), string.Format("%{0}%", role.RoleCode));

                }
                if (!string.IsNullOrEmpty(role.RoleName))
                {
                    sqlBuilder.Append("and r.RoleName like :roleName ");
                    parameterBuilder.AddParameterData("roleName", typeof(string), string.Format("%{0}%", role.RoleName));
                }
            }
         
 

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);
            if (!isCount)
            {

                query.AddScalar("RoleID", NHibernateUtil.Int16);
                query.AddScalar("UpdDate", NHibernateUtil.DateTime);
                query.AddScalar("RoleName", NHibernateUtil.String);
                query.AddScalar("RoleCode", NHibernateUtil.String);
                query.AddScalar("Active", NHibernateUtil.Boolean);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(SuRole)));

                //IList<SS.SU.DTO.ValueObject.RoleLang> list = query.SetResultTransformer(Transformers.AliasToBean(typeof(SS.SU.DTO.ValueObject.RoleLang)))
                //    .List<SS.SU.DTO.ValueObject.RoleLang>();
            }
            else
            {
                //query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
                query.AddScalar("RoleCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }
            return query;
        }


        public IList<SuRole> GetRoleList(SuRole role, short languageId, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<SuRole>(QueryProvider.SuRoleQuery, "FindByRoleCriteria", new object[] { role, sortExpression, false, languageId }, firstResult, maxResult, sortExpression);
        }


        public int CountByRoleCriteria(SuRole role)
        {
            return NHibernateQueryHelper.CountByCriteria(QueryProvider.SuRoleQuery, "FindByRoleCriteria", new object[] { role, string.Empty, true, Convert.ToInt16(0) });
        }

   

        #endregion

        #region public IList<SuRole> FindUserRoleCriteria(long userID)
        public IList<SuRole> FindUserRoleCriteria(long userID)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.AppendLine(" select ");
            sqlBuilder.AppendLine(" b.RoleID as RoleID, ");
            sqlBuilder.AppendLine(" b.ReceiveDocument as ReceiveDocument, ");
            sqlBuilder.AppendLine(" b.VerifyDocument as VerifyDocument, ");
            sqlBuilder.AppendLine(" b.ApproveVerifyDocument as ApproveVerifyDocument, ");
            sqlBuilder.AppendLine(" b.VerifyPayment as VerifyPayment, ");
            sqlBuilder.AppendLine(" b.ApproveVerifyPayment as ApproveVerifyPayment, ");
            sqlBuilder.AppendLine(" b.CounterCashier as CounterCashier ");
            sqlBuilder.AppendLine(" b.AllowMultipleApprovePayment as AllowMultipleApprovePayment");
            sqlBuilder.AppendLine(" b.AllowMultipleApproveAccountant as AllowMultipleApproveAccountant");
            sqlBuilder.AppendLine(" from SuUserRole a inner join SuRole b ");
            sqlBuilder.AppendLine(" on a.RoleID = b.RoleID ");
            sqlBuilder.AppendLine(" and b.Active = 1 ");

            if (userID > 0)
            {
                sqlBuilder.AppendLine(" where a.UserID = :UserID ");
                parameterBuilder.AddParameterData("UserID", typeof(long), userID);
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);

            query.AddScalar("RoleID", NHibernateUtil.Int16);
            query.AddScalar("ReceiveDocument", NHibernateUtil.Boolean);
            query.AddScalar("VerifyDocument", NHibernateUtil.Boolean);
            query.AddScalar("ApproveVerifyDocument", NHibernateUtil.Boolean);
            query.AddScalar("ApproveVerifyPayment", NHibernateUtil.Boolean);
            query.AddScalar("CounterCashier", NHibernateUtil.Boolean);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(SuRole))).List<SuRole>();
        }
        #endregion public IList<SuRole> FindUserRoleCriteria(long userID)

        #region User Group
        public ISQLQuery FindUserGroupByCriteria(SuRole role, bool isCount, string sortExpression)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            if (!isCount)
            {
                sqlBuilder.Append("Select r.RoleId as RoleId,r.RoleCode as RoleCode,r.RoleName as RoleName,r.Active as Active ");
                sqlBuilder.Append("FROM SuRole r ");
                sqlBuilder.Append("WHERE r.RoleCode Like :RoleCode ");
                sqlBuilder.Append("AND r.RoleName Like :RoleName ");
                sqlBuilder.Append("AND r.Active = 1 ");

                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine("ORDER BY r.RoleCode,r.RoleName");
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));

            }
            else
            {
                sqlBuilder.Append("Select COUNT(r.RoleId) as UserGroupCount ");
                sqlBuilder.Append("FROM SuRole r ");
                sqlBuilder.Append("WHERE r.RoleCode Like :RoleCode ");
                sqlBuilder.Append("AND r.RoleName Like :RoleName ");
                sqlBuilder.Append("AND r.Active = 1 ");
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("RoleCode", typeof(string), string.Format("%{0}%", role.RoleCode));
            queryParameterBuilder.AddParameterData("RoleName", typeof(string), string.Format("%{0}%", role.RoleName));
            queryParameterBuilder.FillParameters(query);

            if (!isCount)
            {
                query.AddScalar("RoleID", NHibernateUtil.Int16);
                query.AddScalar("RoleCode", NHibernateUtil.String);
                query.AddScalar("RoleName", NHibernateUtil.String);
                query.AddScalar("Active", NHibernateUtil.Boolean);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(SuRole)));
            }
            else
            {
                query.AddScalar("UserGroupCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }
            return query;
        }

        public IList<SuRole> GetUserGroupList(SuRole userGroup, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<SuRole>(QueryProvider.SuRoleQuery, "FindUserGroupByCriteria", new object[] { userGroup, false, sortExpression }, firstResult, maxResult, sortExpression);
        }

        public int CountUserGroupByCriteria(SuRole userGroup)
        {
            return NHibernateQueryHelper.CountByCriteria(QueryProvider.SuRoleQuery, "FindUserGroupByCriteria", new object[] { userGroup, true, string.Empty });
        }
        #endregion


    }
}
