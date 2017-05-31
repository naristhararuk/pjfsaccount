using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.DB.DTO;
using SCG.DB.DTO.ValueObject;
using SS.Standard.Data.NHibernate.QueryCreator;
using NHibernate;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.QueryDao;

namespace SCG.DB.Query.Hibernate
{
    public class DbAccountLangQuery : NHibernateQueryBase<DbAccountLang, long>, IDbAccountLangQuery
    {
        public IList<AccountLang> FindByDbAccountLangKey(long accountId, short languageId)
        {
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append(" SELECT ");
            sqlBuilder.Append("     DbAccount.AccountID        AS AccountID ,");
            sqlBuilder.Append("     DbAccount.ExpenseGroupID   AS ExpenseGroupID ,");
            sqlBuilder.Append("     DbAccount.AccountCode      AS AccountCode ,");
            sqlBuilder.Append("     DbAccount.Active           AS Active ,");
            sqlBuilder.Append("     DbAccountLang.AccountName  AS AccountName, ");
            sqlBuilder.Append("     DbExpenseGroupLang.Description   AS Description ");
            sqlBuilder.Append(" FROM DbAccount ");
            sqlBuilder.Append("     INNER JOIN DbAccountLang ON ");
            sqlBuilder.Append("         DbAccount.AccountID        =  DbAccountLang.AccountID AND ");
            sqlBuilder.Append("         DbAccount.AccountID        = :AccountId AND ");
            sqlBuilder.Append("         DbAccountLang.LanguageID   = :LanguageId ");
            sqlBuilder.Append("     LEFT JOIN DbExpenseGroupLang ON ");
            sqlBuilder.Append("         DbAccount.ExpenseGroupID   = DbExpenseGroupLang.ExpenseGroupID AND ");
            sqlBuilder.Append("         DbExpenseGroupLang.LanguageID = :LanguageId AND ");
            sqlBuilder.Append("         DbAccount.Active   = 1 ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            queryParameterBuilder.AddParameterData("AccountId", typeof(Int64), accountId);
            queryParameterBuilder.AddParameterData("LanguageId", typeof(Int16), languageId);
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("AccountID", NHibernateUtil.Int64);
            query.AddScalar("ExpenseGroupID", NHibernateUtil.Int64);
            query.AddScalar("AccountCode", NHibernateUtil.String);
            query.AddScalar("AccountName", NHibernateUtil.String);
            query.AddScalar("Description", NHibernateUtil.String);

            query.SetResultTransformer(Transformers.AliasToBean(typeof(AccountLang)));
            IList<AccountLang> list = query.List<AccountLang>();

            return list;
        }

        public IList<AccountLang> FindAccountLangByAccountID(long accountID)
        {
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append(" SELECT    DbLanguage.LanguageID as LanguageId , DbLanguage.LanguageName as LanguageName ,");
            sqlBuilder.Append(" DbAccountLang.AccountLangID as AccountLangID ,  ");
            sqlBuilder.Append(" DbAccountLang.AccountID as AccountID , DbAccountLang.AccountName as AccountName , DbAccountLang.Comment as Comment , ");
            sqlBuilder.Append(" DbAccountLang.Active as Active ");
            sqlBuilder.Append(" FROM  DbLanguage LEFT JOIN  ");
            sqlBuilder.Append(" DbAccountLang ON DbLanguage.LanguageID = DbAccountLang.LanguageID and DbAccountLang.AccountID = :accountID ");
            queryParameterBuilder.AddParameterData("accountID", typeof(long), accountID);




            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("LanguageId", NHibernateUtil.Int16)
                 .AddScalar("LanguageName", NHibernateUtil.String)
                 .AddScalar("AccountLangID", NHibernateUtil.Int64)
                 .AddScalar("AccountID", NHibernateUtil.Int64)
                 .AddScalar("AccountName", NHibernateUtil.String)
                 .AddScalar("Comment", NHibernateUtil.String)
                 .AddScalar("Active", NHibernateUtil.Boolean);

            query.SetResultTransformer(Transformers.AliasToBean(typeof(AccountLang)));


            return query.SetResultTransformer(Transformers.AliasToBean(typeof(AccountLang))).List<AccountLang>(); ;

        }

    }
}
