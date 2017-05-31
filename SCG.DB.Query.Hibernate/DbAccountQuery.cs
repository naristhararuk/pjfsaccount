using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.Query;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;
using SCG.DB.DTO;
using SCG.DB.Query;
using SCG.DB.DTO.ValueObject;
using SS.Standard.Security;

namespace SCG.DB.Query.Hibernate
{
    public class DbAccountQuery : NHibernateQueryBase<DbAccount, long>, IDbAccountQuery
    {
        public IUserAccount UserAccount { get; set; }
        #region  public ISQLQuery FindByAccountCriteria(bool isCount, short languageId, string sortExpression)
        public ISQLQuery FindByAccountCriteria(bool isCount, short languageId, string sortExpression)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            if (!isCount)
            {
                sqlBuilder.Append(" SELECT ");
                sqlBuilder.Append("     DbAccount.AccountID        AS AccountID ,");
                sqlBuilder.Append("     DbAccount.ExpenseGroupID   AS ExpenseGroupID ,");
                sqlBuilder.Append("     DbAccount.AccountCode      AS AccountCode ,");
                sqlBuilder.Append("     DbAccount.Active           AS Active ,");
                sqlBuilder.Append("     DbAccountLang.AccountName  AS AccountName ");
                sqlBuilder.Append(" FROM DbAccount ");
                sqlBuilder.Append("     INNER JOIN DbAccountLang ON ");
                sqlBuilder.Append("         DbAccount.AccountID        =  DbAccountLang.AccountID AND ");
                sqlBuilder.Append("         DbAccountLang.LanguageID   = :LanguageId AND ");
                sqlBuilder.Append("         DbAccount.Active   = 1 ");

                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine("ORDER BY DbAccount.AccountCode,DbAccountLang.AccountName,DbAccount.Active");
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));

            }
            else
            {
                sqlBuilder.Append(" SELECT COUNT(*) AS AccountCount FROM DbAccount ");
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            if (!isCount)
            {
                QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
                queryParameterBuilder.AddParameterData("LanguageId", typeof(Int16), languageId);
                queryParameterBuilder.FillParameters(query);
                query.AddScalar("AccountID", NHibernateUtil.Int64);
                query.AddScalar("AccountCode", NHibernateUtil.String);
                query.AddScalar("Active", NHibernateUtil.Boolean);
                query.AddScalar("AccountName", NHibernateUtil.String);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(AccountLang)));
            }
            else
            {
                query.AddScalar("AccountCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }
            return query;
        }
        #endregion  public ISQLQuery FindByAccountCriteria(bool isCount, short languageId, string sortExpression)

        #region public IList<AccountLang> GetAccountList(short languageId, int firstResult, int maxResult, string sortExpression)
        public IList<AccountLang> GetAccountList(short languageId, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<AccountLang>(ScgDbQueryProvider.DbAccountQuery, "FindByAccountCriteria", new object[] { false, languageId, sortExpression }, firstResult, maxResult, sortExpression);
        }
        #endregion public IList<AccountLang> GetAccountList(short languageId, int firstResult, int maxResult, string sortExpression)

        #region public int CountByAccountCriteria()
        public int CountByAccountCriteria()
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbAccountQuery, "FindByAccountCriteria", new object[] { true, Convert.ToInt16(0), string.Empty });
        }
        #endregion public int CountByAccountCriteria()

        #region Lov
        public ISQLQuery FindByAccountLovCriteria(bool isCount, short languageId, string sortExpression, string expenseGroupID, string accountCode, string description, long companyID, string withoutExpenseCode)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();

            if (!isCount)
                sqlBuilder.Append("SELECT AccountID, ExpenseGroupID, AccountCode, Active, AccountName, Description ");
            else
                sqlBuilder.Append("SELECT COUNT(*) AS AccountCount ");
            
            sqlBuilder.Append(" FROM (SELECT DISTINCT ");
            sqlBuilder.Append("     DbAccount.AccountID        AS AccountID ,");
            sqlBuilder.Append("     DbAccount.ExpenseGroupID   AS ExpenseGroupID ,");
            sqlBuilder.Append("     DbAccount.AccountCode      AS AccountCode ,");
            sqlBuilder.Append("     DbAccount.Active           AS Active ,");
            sqlBuilder.Append("     DbAccountLang.AccountName  AS AccountName, ");
            sqlBuilder.Append("     DbExpenseGroupLang.Description   AS Description ");
            sqlBuilder.Append(" FROM DbAccount ");
            sqlBuilder.Append("     INNER JOIN DbAccountLang ON ");
            sqlBuilder.Append("         DbAccount.AccountID        =  DbAccountLang.AccountID AND ");
            sqlBuilder.Append("         DbAccountLang.LanguageID   = :LanguageId AND ");
            sqlBuilder.Append("         DbAccount.Active   = 1 ");
            sqlBuilder.Append("         inner join DbAccountCompany    ");
            sqlBuilder.Append("         on DbAccountCompany.AccountID = DbAccount.AccountID ");
            sqlBuilder.Append("     INNER JOIN DbExpenseGroup ON ");
            sqlBuilder.Append("         DbAccount.ExpenseGroupID   = DbExpenseGroup.ExpenseGroupID ");
            sqlBuilder.Append("     LEFT JOIN DbExpenseGroupLang ON ");
            sqlBuilder.Append("         DbExpenseGroup.ExpenseGroupID   = DbExpenseGroupLang.ExpenseGroupID AND ");
            sqlBuilder.Append("         DbExpenseGroupLang.LanguageID    = :LanguageId ");
            sqlBuilder.Append(" WHERE DbAccount.Active = '1' ");

            if (!string.IsNullOrEmpty(expenseGroupID))
            {
                sqlBuilder.Append(" AND DbAccount.ExpenseGroupID = :ExpenseGroupID ");
                parameterBuilder.AddParameterData("ExpenseGroupID", typeof(short), short.Parse(expenseGroupID));
            }
            if (!string.IsNullOrEmpty(accountCode))
            {
                sqlBuilder.Append(" AND DbAccount.AccountCode LIKE :AccountCode ");
                parameterBuilder.AddParameterData("AccountCode", typeof(string), "0000" + accountCode + "%");
            }
            if (!string.IsNullOrEmpty(description))
            {
                sqlBuilder.Append(" AND DbAccountLang.AccountName LIKE :Description ");
                parameterBuilder.AddParameterData("Description", typeof(string), "%" + description + "%");
            }
            if (companyID > 0)
            {
                sqlBuilder.Append(" AND DbAccountCompany.CompanyID = :companyID ");
                parameterBuilder.AddParameterData("companyID", typeof(long), companyID);
            }
            if (!string.IsNullOrEmpty(withoutExpenseCode))
            {
                sqlBuilder.Append(" AND DbAccount.AccountCode NOT IN (" + withoutExpenseCode + ")");
            }

            sqlBuilder.Append(" )t1");

            parameterBuilder.AddParameterData("LanguageId", typeof(short), languageId);

            if (!isCount)
            {
                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine(" ORDER BY AccountID, ExpenseGroupID, AccountCode ");
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));
            }
            
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);

            if (!isCount)
            {
                query.AddScalar("AccountID", NHibernateUtil.Int64);
                query.AddScalar("ExpenseGroupID", NHibernateUtil.Int64);
                query.AddScalar("AccountCode", NHibernateUtil.String);
                query.AddScalar("Description", NHibernateUtil.String);
                query.AddScalar("AccountName", NHibernateUtil.String);
                query.AddScalar("Active", NHibernateUtil.Boolean);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(AccountLang)));
            }
            else
            {
                query.AddScalar("AccountCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }

            return query;
        }

        public IList<AccountLang> GetAccountLovList(short languageId, int firstResult, int maxResult, string sortExpression, string expenseGroupID, string accountCode, string description, long companyID, string withoutExpenseCode)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<AccountLang>(ScgDbQueryProvider.DbAccountQuery, "FindByAccountLovCriteria", new object[] { false, languageId, sortExpression, expenseGroupID, accountCode, description, companyID, withoutExpenseCode }, firstResult, maxResult, sortExpression);
        }

        public int CountByAccountLovCriteria(short languageId, string expenseGroupID, string accountCode, string description, long companyID, string withoutExpensCode)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbAccountQuery, "FindByAccountLovCriteria", new object[] { true, languageId, string.Empty, expenseGroupID, accountCode, description, companyID, withoutExpensCode });
        }
        #endregion Lov

        #region AutoComplete
        public IList<AccountLang> FindAutoComplete(string prefixText, AccountAutoCompleteParameter param)
        {
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select distinct ac.AccountID as AccountID, ac.AccountCode as AccountCode, acl.AccountName as AccountName ");
            sqlBuilder.Append(" from DbAccount ac ");
            sqlBuilder.Append(" inner join DbAccountLang acl on acl.AccountID = ac.AccountID and acl.LanguageID = :LanguageID ");
            sqlBuilder.Append(" inner join dbo.DbAccountCompany accC ");
            sqlBuilder.Append(" on accC.AccountID = ac.AccountID  ");
            sqlBuilder.Append(" inner join DbExpenseGroup exG ");
            sqlBuilder.Append(" on exG.ExpenseGroupID = ac.ExpenseGroupID ");

            StringBuilder whereClauseBuilder = new StringBuilder();
            whereClauseBuilder.Append(" and ac.Active = 1 ");
            if (!string.IsNullOrEmpty(prefixText))
            {
                whereClauseBuilder.Append(" and ((ac.AccountCode Like :Account) or (acl.AccountName Like :Account)) ");
                queryParameterBuilder.AddParameterData("Account", typeof(string), String.Format("0000{0}%", prefixText));
            }
            if ((param.ExpenseGroupID) != null && param.ExpenseGroupID > 0)
            {
                whereClauseBuilder.Append(" and ac.ExpenseGroupID = :ExpenseGroupID ");
                queryParameterBuilder.AddParameterData("ExpenseGroupID", typeof(string), param.ExpenseGroupID);
            }
            if ((param.CompanyID) != null && param.CompanyID > 0)
            {
                whereClauseBuilder.Append(" and accC.CompanyID = :CompanyID ");
                queryParameterBuilder.AddParameterData("CompanyID", typeof(long), param.CompanyID);
            }
            if (!string.IsNullOrEmpty(param.WithoutExpenseCode))
            {
                whereClauseBuilder.Append(" and ac.AccountCode not in (" + param.WithoutExpenseCode + ") ");
            }

            if (!string.IsNullOrEmpty(whereClauseBuilder.ToString()))
            {
                sqlBuilder.Append(String.Format(" where 1=1 {0} ", whereClauseBuilder.ToString()));
            }

            queryParameterBuilder.AddParameterData("LanguageID", typeof(Int16), param.LanguageID);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("AccountID", NHibernateUtil.Int64)
                .AddScalar("AccountCode", NHibernateUtil.String)
                .AddScalar("AccountName", NHibernateUtil.String);

            query.SetResultTransformer(Transformers.AliasToBean(typeof(AccountLang)));
            IList<AccountLang> list = query.List<AccountLang>();

            return list;
        }
        #endregion

        #region for Simple Expense
        public IList<ExpenseRecommend> FindExpenseRecommendByExpenseGroup(ExpenseRecommend recommend)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select distinct ac.AccountID as AccountID, ac.AccountCode as AccountCode, acl.AccountName,ac.DomesticRecommend, ac.ForeignRecommend,cost.CostCenterID as CostCenterID, cost.CostCenterCode as CostCenterCode ");
            sqlBuilder.Append(" from DbAccount ac ");
            sqlBuilder.Append(" inner join DbAccountLang acl on ac.AccountID = acl.AccountID and acl.LanguageID = :LangugeID ");
            sqlBuilder.Append(" inner join DbExpenseGroup eg on ac.ExpenseGroupID = eg.ExpenseGroupID and eg.Active = 1 ");
            sqlBuilder.Append(" inner join DbAccountCompany acc on ac.AccountID = acc.AccountID ");
            sqlBuilder.Append(" inner join SuUser u on u.CompanyID = acc.CompanyID and u.UserID = :UserID ");
            sqlBuilder.Append(" inner join DbCostCenter cost on cost.CostCenterID = u.CostCenterID ");
            sqlBuilder.Append(" where ac.Active = 1 ");

            parameterBuilder.AddParameterData("UserID", typeof(long), recommend.UserID);
            parameterBuilder.AddParameterData("LangugeID", typeof(short), recommend.LanguageID);
            if (recommend.IsDomesticRecommend)
            {
                sqlBuilder.Append(" and DomesticRecommend = 1 ");
            }
            else if (recommend.IsForegnRecommend)
            {
                sqlBuilder.Append(" and ForeignRecommend = 1 ");
            }
            sqlBuilder.Append(" order by cost.CostCenterCode, ac.AccountCode, acl.AccountName ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);
            query.AddScalar("CostCenterID", NHibernateUtil.Int64)
                .AddScalar("CostCenterCode", NHibernateUtil.String)
                .AddScalar("AccountID", NHibernateUtil.Int64)
                .AddScalar("AccountCode", NHibernateUtil.String)
                .AddScalar("AccountName", NHibernateUtil.String);

            query.SetResultTransformer(Transformers.AliasToBean(typeof(ExpenseRecommend)));

            return query.List<ExpenseRecommend>();
        }
        #endregion


        public IList<AccountLang> GetAccountInfoByAccount(long acount, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<AccountLang>(ScgDbQueryProvider.DbAccountQuery, "FindAccountInfoByAccount", new object[] { acount, false, sortExpression }, firstResult, maxResult, sortExpression);
        }
        public int CountAccountInfo(long acount)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbAccountQuery, "FindAccountInfoByAccount", new object[] { acount, true, string.Empty });
        }
        public ISQLQuery FindAccountInfoByAccount(long acount, bool isCount, string sortExpression)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            if (!isCount)
            {
                sqlBuilder.Append(" select a.AccountID as AccountID, a.ExpenseGroupID as ExpenseGroupID , ");
                sqlBuilder.Append("  a.AccountCode as AccountCode, a.Active as Active, ");
                sqlBuilder.Append(" al.AccountLangID as AccountLangID ,al.LanguageID as LanguageId, ");
                sqlBuilder.Append(" al.AccountName as AccountName ");
            }
            else
            {
                sqlBuilder.Append(" select count(a.AccountID) as Count ");
            }
            sqlBuilder.Append(" from DbAccount a ");
            sqlBuilder.Append(" left join DbAccountLang al on a.AccountID=al.AccountID and al.LanguageID=:langId ");
            queryParameterBuilder.AddParameterData("langId", typeof(short), UserAccount.CurrentLanguageID);

            #region WhereClause
            StringBuilder whereClauseBuilder = new StringBuilder();
            if (acount != 0)
            {
                whereClauseBuilder.Append(" and a.ExpenseGroupID = :account ");
                queryParameterBuilder.AddParameterData("account", typeof(long), acount);
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
                    sqlBuilder.Append(" order by AccountCode ,AccountName ");
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

                query.AddScalar("AccountID", NHibernateUtil.Int64)
                    .AddScalar("ExpenseGroupID", NHibernateUtil.Int64)
                    .AddScalar("AccountCode", NHibernateUtil.String)
                    .AddScalar("Active", NHibernateUtil.Boolean)
                    .AddScalar("LanguageId", NHibernateUtil.Int16)
                     .AddScalar("AccountName", NHibernateUtil.String)
                    .AddScalar("AccountLangID", NHibernateUtil.Int64);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(AccountLang)));
            }

            return query;
        }


        public DbAccount FindAccountByEGroupIDAID(long expenseID, long accoutID)
        {

            ISQLQuery query = GetCurrentSession().CreateSQLQuery("SELECT  *  FROM  DbAccount  WHERE [ExpenseGroupID] = :expenseID and [AccountID] = :accoutID");
            query.SetInt64("expenseID", expenseID);
            query.SetInt64("accoutID", accoutID);
            query.AddEntity(typeof(DbAccount));
            IList<DbAccount> user = query.List<DbAccount>();
            if (user.Count > 0)
            {
                return user[0];
            }
            else
            {
                return null;
            }


        }

        public DbAccount FindDbAccountByAccountCode(string accountCode)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(DbAccount), "a");
            criteria.Add(Expression.Eq("a.AccountCode", accountCode));

            return criteria.UniqueResult<DbAccount>();
        }

        public DbAccount FindAccountByAccountCodeExpenseGroup(string accountCode, long? companyId)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            StringBuilder sql = new StringBuilder();
            sql.Append(@" SELECT distinct a.AccountID as AccountID FROM  DbAccount a 
                    inner join DbExpenseGroup b on a.ExpenseGroupID = b.ExpenseGroupID 
            ");

            StringBuilder whereClauseBuilder = new StringBuilder();
            whereClauseBuilder.Append(" WHERE a.Active=1 and a.AccountCode = :AccountCode  ");
            if (companyId.HasValue)
            {
                sql.Append(" inner join DbAccountCompany c on a.AccountID = c.AccountID ");
                whereClauseBuilder.Append(" and c.CompanyID = :CompanyID ");
                parameterBuilder.AddParameterData("CompanyID", typeof(long), companyId);
            }

            if (!string.IsNullOrEmpty(whereClauseBuilder.ToString()))
            {
                sql.Append(whereClauseBuilder.ToString());
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());


            parameterBuilder.AddParameterData("AccountCode", typeof(string), accountCode);

            parameterBuilder.FillParameters(query);

            query.AddScalar("AccountID", NHibernateUtil.Int64);

            IList<DbAccount> list = query.SetResultTransformer(Transformers.AliasToBean(typeof(DbAccount))).List<DbAccount>();
            if (list.Count > 0)
                return list.ElementAt<DbAccount>(0);
            else
                return null;
        }

        public string GetAccountCodeExpMapping(string accountCode, string expenseGroupType)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            string sql = "SELECT TOP 1 AccountCodeExpUse FROM DbAccountMapping WHERE AccountCodeEcc = :accountCode ";

            if (!string.IsNullOrEmpty(expenseGroupType))
            {
                sql += " and (ExpenseGroupType IN (:expenseGroupType, '2')) ";
                parameterBuilder.AddParameterData("expenseGroupType", typeof(string), expenseGroupType);
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql);
            parameterBuilder.AddParameterData("accountCode", typeof(string), accountCode);
            parameterBuilder.FillParameters(query);

            query.AddScalar("AccountCodeExpUse", NHibernateUtil.String);

            return query.UniqueResult<string>();
        }
    }
}
