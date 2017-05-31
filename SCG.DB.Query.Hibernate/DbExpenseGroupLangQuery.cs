using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.DB.DTO;
using NHibernate;
using SS.Standard.Data.NHibernate.QueryCreator;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.DB.DTO.ValueObject;
using SCG.DB.DTO.ValueObject;

namespace SCG.DB.Query.Hibernate
{
    public class DbExpenseGroupLangQuery : NHibernateQueryBase<DbExpenseGroupLang, long>, IDbExpenseGroupLangQuery
    {
        public IList<TranslatedListItem> FindExpenseGroupByLangCriteria(short languageId)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" SELECT ");
            sqlBuilder.Append("     DbExpenseGroup.ExpenseGroupID   AS ID, ");
            sqlBuilder.Append("     '[' + DbExpenseGroup.ExpenseGroupCode + ']' + ISNULL(DbExpenseGroupLang.Description,'') AS Symbol ");
            sqlBuilder.Append(" FROM ");
            sqlBuilder.Append("     DbExpenseGroup ");
            sqlBuilder.Append("     INNER JOIN DbExpenseGroupLang ON ");
            sqlBuilder.Append("         DbExpenseGroup.ExpenseGroupID = DbExpenseGroupLang.ExpenseGroupID ");
            sqlBuilder.Append(" WHERE 1=1 AND DbExpenseGroupLang.LanguageID   = :LanguageId AND DbExpenseGroup.Active = 1 ");
            sqlBuilder.Append(" Order By DbExpenseGroup.ExpenseGroupCode");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("LanguageId", typeof(Int16), languageId);
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("ID", NHibernateUtil.Int16);
            query.AddScalar("Symbol", NHibernateUtil.String);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(TranslatedListItem)));

            return query.List<TranslatedListItem>();
        }
        public ISQLQuery FindExpenseGroupByCriteria(VOExpenseGroup criteria, short languageId, bool isCount, string sortExpression)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();

            StringBuilder sqlBuilder = new StringBuilder();

            if (isCount)
            {
                sqlBuilder.Append(" select count(*) as Count ");
            }
            else
            {
                sqlBuilder.Append(" select e.ExpenseGroupID as ExpenseGroupID, el.ExpenseGroupName as ExpenseGroupName, e.Description as Description, e.Active as Active , ");
            }

            sqlBuilder.Append(" from DbExpenseGroup e ");
            sqlBuilder.Append(" inner join DbExpenseGroupLang el on e.ExpenseGroupID = el.ExpenseGroupID and el.LanguageID = :LanguageID ");

            parameterBuilder.AddParameterData("LanguageID", typeof(short),languageId);

            StringBuilder whereClauseBuilder = new StringBuilder();
            whereClauseBuilder.Append(" where 1=1 ");

            if (!string.IsNullOrEmpty(criteria.ExpenseGroupName))
            {
                whereClauseBuilder.Append(" and el.ExpenseGroupName Like :ExpenseGroupName ");
                parameterBuilder.AddParameterData("ExpenseGroupName", typeof(string), String.Format("%{0}%", criteria.ExpenseGroupName));
            }
            if (!string.IsNullOrEmpty(criteria.Description))
            {
                whereClauseBuilder.Append(" and e.Description Like :Description ");
                parameterBuilder.AddParameterData("Description", typeof(string), String.Format("%{0}%", criteria.Description));
            }

            sqlBuilder.Append(whereClauseBuilder.ToString());
            if (!isCount)
            {
                if (!string.IsNullOrEmpty(sortExpression))
                {
                    sqlBuilder.Append(String.Format(" order by {0} ", sortExpression));
                }
                else
                {
                    sqlBuilder.Append(" order by e.ExpenseGroupID, el.ExpenseGroupName, e.Description, e.Active");
                }
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);

            if (!isCount)
            {
                query.AddScalar("ExpenseGroupID", NHibernateUtil.Int64)
                    .AddScalar("ExpenseGroupName", NHibernateUtil.String)
                    .AddScalar("Description", NHibernateUtil.String)
                    .AddScalar("Active", NHibernateUtil.Boolean);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(VOExpenseGroup)));
            }
            else
            {
                query.AddScalar("Count", NHibernateUtil.Int32);
            }
            return query;
        }
        public IList<VOExpenseGroup> GetExpenseGroupList(VOExpenseGroup criteria, short languageId, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<VOExpenseGroup>(
                ScgDbQueryProvider.DbExpenseGroupLangQuery,
                "FindExpenseGroupByCriteria",
                new object[] { criteria, languageId, false, sortExpression },
                firstResult, maxResult, sortExpression);
        }
        public int GetExpenseGroupCount(VOExpenseGroup criteria, short languageId)
        {
            return NHibernateQueryHelper.CountByCriteria(
                    ScgDbQueryProvider.DbExpenseGroupLangQuery,
                    "FindExpenseGroupByCriteria",
                    new object[] { criteria, languageId, true, string.Empty });
        }
        public IList<VOExpenseGroup> FindExpenseGroupAllLanguage(long expenseGroupId)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select l.LanguageID as LanguageID, l.LanguageName as LanguageName, ");
            sqlBuilder.Append(" e.ExpenseGroupName as ExpenseGroupName, e.Active as Active ");
            sqlBuilder.Append(" from DbLanguage l ");
            sqlBuilder.Append(" left join DbExpenseGroupLang e on l.LanguageId = e.LanguageID and e.ExpenseGroupID = :ExpenseGroupID ");

            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("ExpenseGroupID", typeof(long), expenseGroupId);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);

            query.AddScalar("LanguageID", NHibernateUtil.Int16)
                .AddScalar("LanguageName", NHibernateUtil.String)
                .AddScalar("ExpenseGroupName", NHibernateUtil.String)
                .AddScalar("Active", NHibernateUtil.Boolean);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(VOExpenseGroup)));

            return query.List<VOExpenseGroup>(); ;
        }

        public IList<VOExpenseGroup> FindExpenseGroupLang(long epLangId) 
        {
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append(" SELECT    DbLanguage.LanguageID as LanguageID , DbLanguage.LanguageName as LanguageName ,");
            sqlBuilder.Append(" DbExpenseGroupLang.ExpenseGroupLangID as ExpenseGroupLangID , DbExpenseGroupLang.LanguageID AS ExpenseLang , ");
            sqlBuilder.Append(" DbExpenseGroupLang.ExpenseGroupID as ExpenseGroupID , DbExpenseGroupLang.Description as Description , DbExpenseGroupLang.Comment as Comment , ");
            sqlBuilder.Append(" DbExpenseGroupLang.Active as Active ");
            sqlBuilder.Append(" FROM  DbLanguage LEFT JOIN  ");
            sqlBuilder.Append(" DbExpenseGroupLang ON DbLanguage.LanguageID = DbExpenseGroupLang.LanguageID and DbExpenseGroupLang.ExpenseGroupID= :EID ");
            queryParameterBuilder.AddParameterData("EID", typeof(long), epLangId);




            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("LanguageID", NHibernateUtil.Int16)
                 .AddScalar("LanguageName", NHibernateUtil.String)
                 .AddScalar("ExpenseGroupLangID", NHibernateUtil.Int64)
                 .AddScalar("ExpenseLang", NHibernateUtil.Int16)
                 .AddScalar("ExpenseGroupID", NHibernateUtil.Int64)
                 .AddScalar("Description", NHibernateUtil.String)
                 .AddScalar("Comment", NHibernateUtil.String)
                 .AddScalar("Active", NHibernateUtil.Boolean);

            query.SetResultTransformer(Transformers.AliasToBean(typeof(VOExpenseGroup)));


            return query.SetResultTransformer(Transformers.AliasToBean(typeof(VOExpenseGroup))).List<VOExpenseGroup>(); ;

        }


    }
}
