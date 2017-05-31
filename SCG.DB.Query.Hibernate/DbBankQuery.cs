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

namespace SCG.DB.Query.Hibernate
{
    public class DbBankQuery : NHibernateQueryBase< DbBank , short >, IDbBankQuery
    {
        #region public ISQLQuery FindByBankCriteria(BankLang bank, bool isCount, short languageId, string sortExpression)
        public ISQLQuery FindByBankCriteria(BankLang bank, bool isCount, short languageId, string sortExpression)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            if (!isCount)
            {
                sqlBuilder.Append(" SELECT ");
                sqlBuilder.Append("     DbBank.BankId           AS BankId       ,");
                sqlBuilder.Append("     DbBank.BankNo           AS BankNo       ,");
                sqlBuilder.Append("     DbBank.Comment          AS Comment     ,");
                sqlBuilder.Append("     DbBank.Active           AS Active      ,");
                sqlBuilder.Append("     DbBankLang.BankName     AS BankName    ,");
                sqlBuilder.Append("     DbBankLang.ABBRName     AS ABBRName     ");
                sqlBuilder.Append(" FROM DbBank ");
                sqlBuilder.Append("     LEFT JOIN DbBankLang ON ");
                sqlBuilder.Append("         DbBank.BankId           =  DbBankLang.BankId AND ");
                sqlBuilder.Append("         DbBankLang.LanguageID   = :LanguageId ");

                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine("ORDER BY DbBank.BankNo,DbBankLang.BankName,DbBankLang.ABBRName,DbBank.Comment,DbBank.Active");
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));

            }
            else
            {
                sqlBuilder.Append(" SELECT COUNT(BankId) AS BankCount FROM DbBank ");
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            if (!isCount)
            {
                QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
                queryParameterBuilder.AddParameterData("LanguageId", typeof(Int16), languageId);
                queryParameterBuilder.FillParameters(query);
                query.AddScalar("BankId", NHibernateUtil.Int16);
                query.AddScalar("BankNo", NHibernateUtil.String);
                query.AddScalar("Comment", NHibernateUtil.String);
                query.AddScalar("Active", NHibernateUtil.Boolean);
                query.AddScalar("BankName", NHibernateUtil.String);
                query.AddScalar("ABBRName", NHibernateUtil.String);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(BankLang)));
            }
            else
            {
                query.AddScalar("BankCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }
            return query;
        }
        #endregion public ISQLQuery FindByBankCriteria(BankLang bank, bool isCount, short languageId, string sortExpression)

        #region public IList<BankLang> GetBankList(BankLang bank , short languageId, int firstResult, int maxResult, string sortExpression)
        public IList<BankLang> GetBankList(BankLang bank , short languageId, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<BankLang>(ScgDbQueryProvider.DbBankQuery, "FindByBankCriteria", new object[] { bank, false, languageId, sortExpression }, firstResult, maxResult, sortExpression);
        }
        #endregion public IList<BankLang> GetBankList(BankLang bank , short languageId, int firstResult, int maxResult, string sortExpression)

        #region public int CountByBankCriteria(BankLang bank)
        public int CountByBankCriteria(BankLang bank)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbBankQuery, "FindByBankCriteria", new object[] { bank, true, Convert.ToInt16(0), string.Empty });
        }
        #endregion public int CountByBankCriteria(BankLang bank)
    }
}
