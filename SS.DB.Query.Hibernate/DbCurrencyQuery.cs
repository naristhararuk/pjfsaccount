using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SS.Standard.Data.NHibernate.QueryDao;

using SS.DB.DTO;
using SS.DB.DTO.ValueObject;
using SS.DB.Query;
using NHibernate;
using NHibernate.Transform;
using SS.Standard.Security;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SS.DB.Query.Hibernate
{
    public class DbCurrencyQuery : NHibernateQueryBase<DbCurrency, short>, IDbCurrencyQuery
    {
        public IUserAccount UserAccount { get; set; }
        public ISQLQuery FindByCurrencyCriteria(DbCurrency currency, string sortExpression, bool isCount)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            if (!isCount)
            {
                sqlBuilder.Append("select c.CurrencyID as CurrencyID,c.Symbol as Symbol,c.Comment as Comment,c.Active as Active from DbCurrency c ");
            }
            else
            {
                sqlBuilder.Append("select count(c.CurrencyId) as CurrencyCount from DbCurrency c ");
            }

            ISQLQuery query;

            if (!isCount)
            {
                if (string.IsNullOrEmpty(sortExpression))
                {
                    sqlBuilder.AppendLine("ORDER BY CurrencyID,Symbol,Comment,Active");
                }
                else
                {
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));
                }
                query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
                query.AddScalar("CurrencyID", NHibernateUtil.Int16)
                    .AddScalar("Symbol", NHibernateUtil.String)
                    .AddScalar("Comment", NHibernateUtil.String)
                    .AddScalar("Active", NHibernateUtil.Boolean);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(DbCurrency)));
            }
            else
            {
                query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
                query.AddScalar("CurrencyCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }
            return query;
        }
        public IList<DbCurrency> GetCurrencyList(DbCurrency currency, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<DbCurrency>(SsDbQueryProvider.DbCurrencyQuery, "FindByCurrencyCriteria", new object[] { currency, sortExpression, false }, firstResult, maxResult, sortExpression);

        }
        public int CountByCurrencyCriteria(DbCurrency currency)
        {
            return NHibernateQueryHelper.CountByCriteria(SsDbQueryProvider.DbCurrencyQuery, "FindByCurrencyCriteria", new object[] { currency, string.Empty, true });
        }
        public DbCurrency FindByCurrencySymbol(string symbol, bool isExpense, bool isAdvanceFR)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine(" FROM DbCurrency ");
            sqlBuilder.AppendLine(" WHERE Active=1 and Symbol = :Symbol ");

            if (!isExpense)
            {
                sqlBuilder.Append(" and Comment = '*' ");
            }
            if (isAdvanceFR)
            {
                sqlBuilder.Append(" and Symbol not in ('THB') ");
            }

            IList<DbCurrency> currencyList = GetCurrentSession().CreateQuery(sqlBuilder.ToString()).SetString("Symbol", symbol).List<DbCurrency>();

            if (currencyList.Count > 0)
            {
                return currencyList[0];
            }
            else
            {
                return null;
            }
        }
        public DbCurrency FindCurrencyById(short CurrencyID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine(" FROM DbCurrency ");
            sqlBuilder.AppendLine(" WHERE CurrencyID = :CurrencyID ");

            IList<DbCurrency> currencyList = GetCurrentSession().CreateQuery(sqlBuilder.ToString()).SetInt16("CurrencyID", CurrencyID).List<DbCurrency>();

            if (currencyList.Count > 0)
            {
                return currencyList[0];
            }
            else
            {
                return null;
            }
        }

        public IList<TranslatedListItem> GetTranslatedListItem()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" SELECT c.CurrencyID as ID, c.Symbol as Symbol ");
            sqlBuilder.Append(" FROM DbCurrency c WHERE c.Active = 'true' ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("ID", NHibernateUtil.Int16)
                .AddScalar("Symbol", NHibernateUtil.String);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(TranslatedListItem))).List<TranslatedListItem>();
        }
        public IList<VOUCurrencySetup> GetCurrencyListByCriteria(VOUCurrencySetup criteria, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<VOUCurrencySetup>(SsDbQueryProvider.DbCurrencyQuery, "FindCurrencyByCriteria", new object[] { criteria, false, sortExpression }, firstResult, maxResult, sortExpression);
        }

        public ISQLQuery FindCurrencyDetail(short id, string sortExpression)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();

            sqlBuilder.Append("select db.CurrencyID as CurrencyID, db.Symbol as Symbol, db.Active as Active, dbl.Description as Description ");

            sqlBuilder.Append(" from DbCurrency db ");
            sqlBuilder.Append(" inner join DbCurrencyLang dbl on db.CurrencyID = dbl.CurrencyID ");
            sqlBuilder.Append(" inner join DbLanguage lang on lang.LanguageID = dbl.LanguageID ");

            #region WhereClause
            StringBuilder whereClauseBuilder = new StringBuilder();
            whereClauseBuilder.Append("  and db.Active = 1 ");
            if (id != null)
            {
                whereClauseBuilder.Append(" and  db.CurrencyID = :id ");
                queryParameterBuilder.AddParameterData("id", typeof(string), String.Format("%{0}%", id));
            }
            #endregion

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("CurrencyID", NHibernateUtil.Int16)
                  .AddScalar("Symbol", NHibernateUtil.String)
                  .AddScalar("Active", NHibernateUtil.Boolean)
                 .AddScalar("Description", NHibernateUtil.String)
                .AddScalar("LanguageID", NHibernateUtil.Int16)
               .AddScalar("Comment", NHibernateUtil.String)
               .AddScalar("LangActive", NHibernateUtil.Boolean)
              .AddScalar("LanguageName", NHibernateUtil.String);

            query.SetResultTransformer(Transformers.AliasToBean(typeof(VOUCurrencySetup)));

            return query;
        }
        public ISQLQuery FindCurrencyByCriteria(VOUCurrencySetup criteria, bool isCount, string sortExpression)
        {

            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            if (!isCount)
            {
                sqlBuilder.Append(" select DbCurrency.CurrencyID as CurrencyID, DbCurrency.Symbol as Symbol,  ");
                sqlBuilder.Append(" DbCurrency.Active as Active, DbCurrencyLang.CurrencyLangID as CurrencyLangID , ");
                sqlBuilder.Append(" DbCurrencyLang.LanguageID as LanguageID ,DbCurrencyLang.Description as Description ");

            }
            else
            {
                sqlBuilder.Append(" select count(DbCurrency.Symbol) as Count ");
            }
            sqlBuilder.Append(" from DbCurrency Left Outer Join  DbCurrencyLang ");
            sqlBuilder.Append(" on DbCurrency.CurrencyID=DbCurrencyLang.CurrencyID and DbCurrencyLang.LanguageID= :langId ");
            queryParameterBuilder.AddParameterData("langId", typeof(short), UserAccount.CurrentLanguageID);

            #region WhereClause
            StringBuilder whereClauseBuilder = new StringBuilder();

            if (!string.IsNullOrEmpty(criteria.Symbol))
            {
                whereClauseBuilder.Append(" and DbCurrency.Symbol like :Symbol ");
                queryParameterBuilder.AddParameterData("Symbol", typeof(string), String.Format("%{0}%", criteria.Symbol));
            }

            if (!string.IsNullOrEmpty(criteria.Description))
            {
                whereClauseBuilder.Append(" and DbCurrencyLang.Description Like :Description ");
                queryParameterBuilder.AddParameterData("Description", typeof(string), String.Format("%{0}%", criteria.Description));
            }

            if (!string.IsNullOrEmpty(whereClauseBuilder.ToString()))
            {
                sqlBuilder.Append(String.Format(" where 1=1 {0} ", whereClauseBuilder.ToString()));
            }
            #endregion
            #region Order By
            if (!isCount)
            {
                if (!string.IsNullOrEmpty(sortExpression))
                {
                    sqlBuilder.Append(String.Format(" order by {0} ", sortExpression));
                }
                else
                {
                    sqlBuilder.Append(" order by DbCurrency.Symbol , DbCurrencyLang.Description ,DbCurrency.Active ");
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

                query.AddScalar("CurrencyID", NHibernateUtil.Int16)
                    .AddScalar("Symbol", NHibernateUtil.String)
                    .AddScalar("CurrencyLangID", NHibernateUtil.Int64)
                    .AddScalar("LanguageID", NHibernateUtil.Int16)
                    .AddScalar("Description", NHibernateUtil.String)
                   .AddScalar("Active", NHibernateUtil.Boolean);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(VOUCurrencySetup)));
            }

            return query;

        }
        public int CountCurrencyByCriteria(VOUCurrencySetup criteria)
        {
            return NHibernateQueryHelper.CountByCriteria(SsDbQueryProvider.DbCurrencyQuery, "FindCurrencyByCriteria", new object[] { criteria, true, string.Empty });
        }

        public IList<VOUCurrencySetup> GetCurrencyListItem(string prefix, CurrencyAutoCompleteParameter param)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();

            sqlBuilder.Append(" select DbCurrency.Symbol as Symbol, DbCurrencyLang.Description as Description ");
            sqlBuilder.Append(" ,DbCurrency.CurrencyID as CurrencyID ");
            sqlBuilder.Append(" from DbCurrency ");
            sqlBuilder.Append(" inner join DbCurrencyLang ");
            sqlBuilder.Append(" on DbCurrency.CurrencyID = DbCurrencyLang.CurrencyID ");
            sqlBuilder.Append(" and LanguageID = :languageID ");
            sqlBuilder.Append(" Where DbCurrency.Active = 1 AND DbCurrencyLang.Active = 1  ");   //เพิ่มเพื่อเอา Currency ที่มี Status เป็น True ครับ

            if (!string.IsNullOrEmpty(prefix))
            {
                sqlBuilder.Append(" and DbCurrency.Symbol like :Prefix ");
                queryParameterBuilder.AddParameterData("Prefix", typeof(string), String.Format("%{0}%", prefix));
            }
            if (!param.IsExpense)
            {
                sqlBuilder.Append(" and DbCurrency.Comment = '*' ");
            }
            if (param.IsAdvanceFR)
            {
                sqlBuilder.Append("and DbCurrency.Symbol not in ('THB')");
            }

            sqlBuilder.Append(" order by DbCurrency.Symbol ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            queryParameterBuilder.AddParameterData("languageID", typeof(short), param.LanguageID);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("Symbol", NHibernateUtil.String)
                .AddScalar("Description", NHibernateUtil.String)
                .AddScalar("CurrencyID", NHibernateUtil.Int16);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(VOUCurrencySetup))).List<VOUCurrencySetup>();
        }
        public VOUCurrencySetup GetCurrencyLangByCurrencyID(short currencyId, short languageId)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();

            sqlBuilder.Append(" select DbCurrency.Symbol as Symbol, DbCurrencyLang.Description as Description ");
            sqlBuilder.Append(" ,DbCurrency.CurrencyID as CurrencyID ");
            sqlBuilder.Append(" from DbCurrency ");
            sqlBuilder.Append(" inner join DbCurrencyLang ");
            sqlBuilder.Append(" on DbCurrency.CurrencyID = DbCurrencyLang.CurrencyID ");
            sqlBuilder.Append(" and LanguageID = :languageID ");
            sqlBuilder.Append(" Where DbCurrency.Active = 1 AND DbCurrencyLang.Active = 1  ");   //เพิ่มเพื่อเอา Currency ที่มี Status เป็น True ครับ
            sqlBuilder.Append(" and DbCurrency.CurrencyID = :CurrencyID ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            queryParameterBuilder.AddParameterData("languageID", typeof(short), languageId);
            queryParameterBuilder.AddParameterData("CurrencyID", typeof(short), currencyId);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("Symbol", NHibernateUtil.String)
                .AddScalar("Description", NHibernateUtil.String)
                .AddScalar("CurrencyID", NHibernateUtil.Int16);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(VOUCurrencySetup))).UniqueResult<VOUCurrencySetup>();
        }
        
    }
}
