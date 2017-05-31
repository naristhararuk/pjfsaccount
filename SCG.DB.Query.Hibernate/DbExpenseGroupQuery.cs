using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.QueryDao;
using SCG.DB.DTO;
using NHibernate;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.QueryCreator;
using SS.Standard.Security;
using SCG.DB.DTO.ValueObject;

namespace SCG.DB.Query.Hibernate
{
    public class DbExpenseGroupQuery : NHibernateQueryBase<DbExpenseGroup, long>, IDbExpenseGroupQuery
    {
        public IUserAccount UserAccount { get; set; }
        public ISQLQuery FindExpenseGroupByCriteria(VOExpenseGroup criteria, bool isCount, string sortExpression)
        {

            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();
            if (!isCount)
            {
                sqlBuilder.Append(" select DbExpenseGroup.ExpenseGroupID as ExpenseGroupID , DbExpenseGroup.ExpenseGroupCode as ExpenseGroupCode, ");
                sqlBuilder.Append(" DbExpenseGroup.Active as Active , ");
                sqlBuilder.Append(" DbExpenseGroupLang.ExpenseGroupLangID as ExpenseGroupLangID, DbExpenseGroupLang.LanguageID as LanguageID ");
                sqlBuilder.Append(" , DbExpenseGroupLang.Description as Description ");

            }
            else
            {
                sqlBuilder.Append(" select count(DbExpenseGroup.ExpenseGroupID ) as Count ");
            }
            sqlBuilder.Append("  from DbExpenseGroup left join DbExpenseGroupLang ");
            sqlBuilder.Append(" on DbExpenseGroup.ExpenseGroupID=DbExpenseGroupLang.ExpenseGroupID and DbExpenseGroupLang.LanguageID= :langId ");
            queryParameterBuilder.AddParameterData("langId", typeof(short), UserAccount.CurrentLanguageID);

            StringBuilder whereClauseBuilder = new StringBuilder();

            if (!string.IsNullOrEmpty(criteria.Description))
            {
                whereClauseBuilder.Append(" and DbExpenseGroupLang.Description like :Description ");
                queryParameterBuilder.AddParameterData("Description", typeof(string), String.Format("%{0}%", criteria.Description));
            }
            if (!string.IsNullOrEmpty(criteria.ExpenseGroupCode))
            {
                whereClauseBuilder.Append(" and DbExpenseGroup.ExpenseGroupCode like :ExpenseGroupCode ");
                queryParameterBuilder.AddParameterData("ExpenseGroupCode", typeof(string), String.Format("%{0}%", criteria.ExpenseGroupCode));
            }

            if (!string.IsNullOrEmpty(whereClauseBuilder.ToString()))
            {
                sqlBuilder.Append(String.Format(" where 1=1 {0} ", whereClauseBuilder.ToString()));
            }

            #region Order By
            if (!isCount)
            {
                if (!string.IsNullOrEmpty(sortExpression))
                {
                    sqlBuilder.Append(String.Format(" order by {0} ", sortExpression));
                }
                else
                {
                    sqlBuilder.Append(" order by DbExpenseGroup.ExpenseGroupCode,DbExpenseGroupLang.Description");
                }
            }
            #endregion

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            queryParameterBuilder.FillParameters(query);

            if (isCount)
            {
                query.AddScalar("Count", NHibernateUtil.Int32);
            }
            else
            {


                query.AddScalar("ExpenseGroupID", NHibernateUtil.Int64)
                    .AddScalar("ExpenseGroupLangID", NHibernateUtil.Int64)
                    .AddScalar("LanguageID", NHibernateUtil.Int16)
                    .AddScalar("ExpenseGroupCode", NHibernateUtil.String)
                    .AddScalar("Description", NHibernateUtil.String)
                    .AddScalar("Active", NHibernateUtil.Boolean);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(VOExpenseGroup)));

            }

            return query;

        }
        public IList<VOExpenseGroup> GetExpenseGroupByCriteria(VOExpenseGroup criteria, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<VOExpenseGroup>(ScgDbQueryProvider.DbExpenseGroupQuery, "FindExpenseGroupByCriteria", new object[] { criteria, false, sortExpression }, firstResult, maxResult, sortExpression);
        }
        public int CountExpenseGroupByCriteria(VOExpenseGroup criteria)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbExpenseGroupQuery, "FindExpenseGroupByCriteria", new object[] { criteria, true, string.Empty });
        }

    }
}
