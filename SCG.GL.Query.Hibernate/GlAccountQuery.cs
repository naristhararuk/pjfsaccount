using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.Query;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SCG.GL.DTO;
using SCG.GL.Query;
using SCG.GL.DTO.ValueObject;

namespace SCG.GL.Query.Hibernate
{
    public class GlAccountQuery : NHibernateQueryBase< GlAccount , short >, IGlAccountQuery
    {
        #region public ISQLQuery FindByAccountCriteria(AccountLang role, bool isCount, short languageId)
        public ISQLQuery FindByAccountCriteria(AccountLang role, bool isCount, short languageId, string sortExpression )
        {
            StringBuilder sqlBuilder = new StringBuilder();
            if (!isCount)
            {
                sqlBuilder.Append(" SELECT ");
                sqlBuilder.Append("     GlAccount.AccId             AS AccId        ,");
                sqlBuilder.Append("     GlAccount.AccNo             AS AccNo        ,");
                sqlBuilder.Append("     GlAccount.AccType           AS AccType      ,");
                sqlBuilder.Append("     GlAccount.AccLevel          AS AccLevel     ,");
                sqlBuilder.Append("     GlAccount.MainAccID         AS MainAccID    ,");
                sqlBuilder.Append("     GlAccount.TransactionYN     AS TransactionYN,");
                sqlBuilder.Append("     GlAccount.Active            AS Active       ,");
                sqlBuilder.Append("     GlAccountLang.AccountName   AS AccountName  ");
                sqlBuilder.Append(" FROM GlAccount ");
                sqlBuilder.Append("     LEFT JOIN GlAccountLang ON ");
                sqlBuilder.Append("         GlAccount.AccId             = GlAccountLang.AccId AND ");
                sqlBuilder.Append("         GlAccountLang.LanguageID=:LanguageId ");

                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine("ORDER BY AccNo,AccountName,AccType,AccLevel,MainAccID,TransactionYN,GlAccount.Active");
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));

            }
            else
            {
                sqlBuilder.Append(" SELECT COUNT(AccId) AS AccCount FROM GlAccount ");
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            if (!isCount)
            {
                QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
                queryParameterBuilder.AddParameterData("LanguageId", typeof(Int16), languageId);
                queryParameterBuilder.FillParameters(query);
                query.AddScalar("AccId", NHibernateUtil.Int16);
                query.AddScalar("AccNo", NHibernateUtil.String);
                query.AddScalar("AccType", NHibernateUtil.Int16);
                query.AddScalar("AccLevel", NHibernateUtil.Int16);
                query.AddScalar("MainAccID", NHibernateUtil.String);
                query.AddScalar("TransactionYN", NHibernateUtil.Boolean);
                query.AddScalar("Active", NHibernateUtil.Boolean);
                query.AddScalar("AccountName", NHibernateUtil.String);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(AccountLang)));
            }
            else
            {
                query.AddScalar("AccCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }
            return query;
        }
        #endregion public ISQLQuery FindByAccountCriteria(AccountLang role, bool isCount, short languageId)

        #region public IList<AccountLang> GetAccoutnList(AccountLang account , short languageId, int firstResult, int maxResult, string sortExpression)
        public IList<AccountLang> GetAccoutnList(AccountLang account, short languageId, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<AccountLang>(GlQueryProvider.GlAccountQuery, "FindByAccountCriteria", new object[] { account, false, languageId, sortExpression }, firstResult, maxResult, sortExpression);
        }
        #endregion public IList<AccountLang> GetAccoutnList(AccountLang account , short languageId, int firstResult, int maxResult, string sortExpression)

        #region public int CountByAccountCriteria(AccountLang role)
        public int CountByAccountCriteria(AccountLang role)
        {
            return NHibernateQueryHelper.CountByCriteria(GlQueryProvider.GlAccountQuery, "FindByAccountCriteria", new object[] { role, true, Convert.ToInt16(0), string.Empty });
        }
        #endregion public int CountByAccountCriteria(AccountLang role)
    }
}
