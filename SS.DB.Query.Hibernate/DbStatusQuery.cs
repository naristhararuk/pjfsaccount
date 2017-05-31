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
    public class DbStatusQuery : NHibernateQueryBase<DbStatus, short>, IDbStatusQuery
    {
        #region IDbStatusQuery Members

        public IList<DbStatus> GetStatusList(int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<DbStatus>(SsDbQueryProvider.DbStatusQuery, "FindByLanguageCriteria", new object[] { sortExpression, false }, firstResult, maxResult, sortExpression);
        }
        public ISQLQuery FindByLanguageCriteria(string sortExpression, bool isCount)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            ISQLQuery query;
            if (isCount)
            {
                sqlBuilder.Append("select count(s.StatusID) as Count ");
                sqlBuilder.Append(" from DbStatus s ");
                //sqlBuilder.Append(" left join DbStatusLang sl on sl.StatusID = s.StatusID ");

                query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
                query.AddScalar("Count", NHibernateUtil.Int16);
            }
            else
            {

                sqlBuilder.Append("select s.StatusID,s.GroupStatus,s.Status,s.comment,s.active ");

                sqlBuilder.Append(" from DbStatus s ");
                //sqlBuilder.Append(" left join DbStatusLang sl on sl.StatusID = s.StatusID ");

                if (string.IsNullOrEmpty(sortExpression))
                {
                    sqlBuilder.Append(" order by s.StatusID, s.GroupStatus, s.Status, s.comment, s.active ");
                }
                else
                {
                    sqlBuilder.Append(string.Format(" order by {0} ", sortExpression));
                }
                query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
                query.AddScalar("StatusID", NHibernateUtil.Int16);
                query.AddScalar("GroupStatus", NHibernateUtil.String);
                query.AddScalar("Status", NHibernateUtil.String);
                query.AddScalar("Comment", NHibernateUtil.String);
                query.AddScalar("active", NHibernateUtil.Boolean);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(DbStatus)));
            }
            return query;
        }
        public int GetCountStatusList()
        {
            return NHibernateQueryHelper.CountByCriteria(SsDbQueryProvider.DbStatusQuery,"FindByLanguageCriteria",new object[] {string.Empty, true });
        }
        public IList<StatusLang> FindPaymentTypeByLang(short languageID)
        {

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT s.StatusID as StatusID,s.Status as Status,sl.StatusDesc as StatusDesc ");
            sqlBuilder.Append("FROM DbStatus as s ");
            sqlBuilder.Append("LEFT JOIN DbStatusLang as sl on s.StatusID = sl.StatusID ");
            sqlBuilder.Append("WHERE s.GroupStatus = 'PaymentTypeDMT' AND sl.LanguageID =:LanguageID AND s.Active = 1 ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("LanguageID", typeof(short), languageID);
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("StatusID", NHibernateUtil.Int16);
            query.AddScalar("StatusDesc", NHibernateUtil.String);
            query.AddScalar("Status", NHibernateUtil.String);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(StatusLang))).List<StatusLang>();


        }
        #endregion
    }
}
