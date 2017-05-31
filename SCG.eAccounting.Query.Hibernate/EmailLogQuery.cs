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

using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.Query;


namespace SCG.eAccounting.Query.Hibernate
{
	public class EmailLogQuery : NHibernateQueryBase<SuEmailLog, long>, IEmailLogQuery
    {
        #region query Advance
        #region ISQLQuery FindByEmailLogCriteria(string emailType,string sendDate,string requestNo,int status, bool isCount, string sortExpression)
        public ISQLQuery FindByEmailLogCriteria(string emailType,string sendDate,string requestNo,int status, bool isCount, string sortExpression,short currentLanguage)
        {
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();
            if (!isCount)
            {
                sqlBuilder.Append(" select EmailLogID,RequestNo,EmailType+' ('+dbStatusLang.StatusDesc+')' as EmailType,SendDate,ToEmail,CCEmail, ");
                sqlBuilder.Append(" (case when SuEMailLog.status = 1 then 'Success' ");
                sqlBuilder.Append(" when SuEMailLog.status = 2 then 'Fail' ");
                sqlBuilder.Append(" else '' end) as StatusName, Remark ");
                sqlBuilder.Append(" from SuEMailLog with (nolock) ");
                sqlBuilder.Append(" LEFT JOIN dbStatus with (nolock) On SuEMailLog.EmailType = dbStatus.STATUS ");
                sqlBuilder.Append(" LEFT JOIN dbStatusLang with (nolock) On dbStatus.StatusID = dbStatusLang.StatusID ");
                sqlBuilder.Append(" WHERE  SuEMailLog.Active =1 AND dbStatusLang.LanguageID = :Language");
                queryParameterBuilder.AddParameterData("Language", typeof(string), currentLanguage);
                if (!emailType.Equals(string.Empty))
                {
                    sqlBuilder.Append(" AND SuEMailLog.EmailType = :emailType");
                    queryParameterBuilder.AddParameterData("emailType", typeof(string), emailType);
                }
                if (!sendDate.Equals(string.Empty))
                {
                    sqlBuilder.Append(" AND Convert(varchar,SuEMailLog.SendDate,103) = Convert(varchar,:sendDate,103)");
                    queryParameterBuilder.AddParameterData("sendDate", typeof(string), sendDate);
                }
                if (!requestNo.Equals(string.Empty))
                {
                    sqlBuilder.Append(" AND SuEMailLog.RequestNo = :requestNo");
                    queryParameterBuilder.AddParameterData("requestNo", typeof(string), requestNo);
                }
                if (status != 0)
                {
                    sqlBuilder.Append(" AND SuEMailLog.Status = :status");
                    queryParameterBuilder.AddParameterData("status", typeof(int), status);
                }

                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine(" ORDER BY SendDate desc, RequestNo");
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));

            }
            else
            {
                sqlBuilder.Append(" SELECT COUNT(EmailLogId) AS LogCount FROM SuEMailLog");
                sqlBuilder.Append(" LEFT JOIN dbStatus On SuEMailLog.EmailType = dbStatus.STATUS ");
                sqlBuilder.Append(" LEFT JOIN dbStatusLang On dbStatus.StatusID = dbStatusLang.StatusID ");
                sqlBuilder.Append(" WHERE  SuEMailLog.Active =1 AND dbStatusLang.LanguageID = :Language");
                queryParameterBuilder.AddParameterData("Language", typeof(string), currentLanguage);
                if (!emailType.Equals(string.Empty))
                {
                    sqlBuilder.Append(" AND SuEMailLog.EmailType = :emailType");
                    queryParameterBuilder.AddParameterData("emailType", typeof(string), emailType);
                }
                if (!sendDate.Equals(string.Empty))
                {
                    sqlBuilder.Append(" AND Convert(varchar,SuEMailLog.SendDate,103) = Convert(varchar,:sendDate,103)");
                    queryParameterBuilder.AddParameterData("sendDate", typeof(string), sendDate);
                }
                if (!requestNo.Equals(string.Empty))
                {
                    sqlBuilder.Append(" AND SuEMailLog.RequestNo = :requestNo");
                    queryParameterBuilder.AddParameterData("requestNo", typeof(string), requestNo);
                }
                if (status != 0)
                {
                    sqlBuilder.Append(" AND SuEMailLog.Status = :status");
                    queryParameterBuilder.AddParameterData("status", typeof(int), status);
                }
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            queryParameterBuilder.FillParameters(query);
            if (!isCount)
            {
                query.AddScalar("RequestNo", NHibernateUtil.String);
                query.AddScalar("EmailType", NHibernateUtil.String);
                query.AddScalar("SendDate", NHibernateUtil.DateTime);
                query.AddScalar("ToEmail", NHibernateUtil.String);
                query.AddScalar("CCEmail", NHibernateUtil.String);
                query.AddScalar("StatusName", NHibernateUtil.String);
                query.AddScalar("Remark", NHibernateUtil.String);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(VOEmailLog)));
            }
            else
            {
                query.AddScalar("LogCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }
            return query;
        }
        #endregion 
        #region IList<SuEmailLog> GetLogList(string emailType, string sendDate, string requestNo, int status, int firstResult, int maxResult, string sortExpression)
        public IList<VOEmailLog> GetLogList(string emailType, string sendDate, string requestNo, int status, int firstResult, int maxResult, string sortExpression,short currentLanguage)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<VOEmailLog>(ScgeAccountingQueryProvider.EmailLogQuery, "FindByEmailLogCriteria", new object[] { emailType, sendDate, requestNo, status, false, sortExpression,currentLanguage }, firstResult, maxResult, sortExpression);
        }
        #endregion
        #region int CountByLogCriteria(string emailType, string sendDate, string requestNo, int status)
        public int CountByLogCriteria(string emailType, string sendDate, string requestNo, int status,short currentLanguage)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgeAccountingQueryProvider.EmailLogQuery, "FindByEmailLogCriteria", new object[] { emailType, sendDate, requestNo, status, true, string.Empty, currentLanguage });
        }
        #endregion
        #endregion

      
    }
}
