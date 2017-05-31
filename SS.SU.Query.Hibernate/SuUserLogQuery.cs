using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;



using SS.SU.Query;

using NHibernate;
using NHibernate.Transform;
using NHibernate.Expression;
using SS.SU.DTO;
using SS.SU.DTO.ValueObject;

namespace SS.SU.Query.Hibernate
{
    public class SuUserLogQuery : NHibernateQueryBase<SuUserLog, long>, ISuUserLogQuery
    {


        #region public IList<SuUserLog> FindSuUserLogByUserIdSessionID(string UserName, string sessionId)
        public IList<SuUserLog> FindSuUserLogByUserIdSessionID(string UserName, string sessionId)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" FROM SuUserLog u WHERE UserName = :UserName AND Sessionid = :SessionID");
            IQuery query = GetCurrentSession().CreateQuery(sqlBuilder.ToString());

            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            
            parameterBuilder.AddParameterData("UserName", typeof(string), UserName);
            parameterBuilder.AddParameterData("SessionID", typeof(string), sessionId);
            parameterBuilder.FillParameters(query);

            return query.List<SuUserLog>();
        }
        #endregion public IList<SuUserLog> FindSuUserLogByUserIdSessionID(string UserName, string sessionId)
 #region ISuUserLogQuery Members


        public object GetUserLogList(string dateFrom, string dateTo, string p, int startRow, int pageSize, string sortExpression)
        {
            throw new NotImplementedException();
        }

        #endregion

       

        public IList<SuUserLoginLog> FindSuUserLoginLogListQuery(DateTime? fromDate, DateTime? toDate, string UserName, string Status, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<SuUserLoginLog>(
                QueryProvider.SuUserLogQuery,
                "FindSuUserLoginLogSearchResult",
                new object[] { fromDate, toDate, UserName,Status, sortExpression, false },
                firstResult,
                maxResult,
                sortExpression);
        }
        public ISQLQuery FindSuUserLoginLogSearchResult(DateTime? fromDate, DateTime? toDate, string UserName, string Status, string sortExpression, bool isCount)
        { 
            StringBuilder strQuery = new StringBuilder();
            ISQLQuery query;
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();

            if (!isCount)
            {
                strQuery.AppendLine(" select SignInDate,Status,UserName ");
                strQuery.AppendLine(" from SuUserLog ");
                strQuery.AppendLine(" WHERE 1=1  ");
                if (!string.IsNullOrEmpty(UserName))
                {
                    strQuery.AppendLine(" and UserName like :UserName ");
                    parameterBuilder.AddParameterData("UserName", typeof(string), string.Format("%{0}%", UserName));
                }
                   
                if (fromDate.HasValue)
                {
                    strQuery.AppendLine(" and SignInDate >= CONVERT(DATETIME, :fromDate, 103)"); //coalesce(cast(nullif(:fromDate,'') as datetime),SignInDate)
                    parameterBuilder.AddParameterData("fromDate", typeof(DateTime), fromDate);
                }
                if (toDate.HasValue)
                {
                    strQuery.AppendLine(" and SignInDate < CONVERT(DATETIME, :toDate, 103) + 1 "); //coalesce(cast(nullif(:toDate,'') as datetime),SignInDate)
                    parameterBuilder.AddParameterData("toDate", typeof(DateTime), toDate);
                }
                //strQuery.AppendLine(" and Status = coalesce(nullif(:Status,''),Status) ");
                if (!string.IsNullOrEmpty(Status))
                {
                    strQuery.AppendLine(" and Status like :Status ");
                    parameterBuilder.AddParameterData("Status", typeof(string), Status);
                }

                if (string.IsNullOrEmpty(sortExpression))
                {
                    strQuery.Append(" order by SignInDate desc,UserName,Status");
                }
                else
                {
                    strQuery.Append(string.Format(" order by {0} ", sortExpression));
                }

                query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
                parameterBuilder.FillParameters(query);
              
                query.AddScalar("SignInDate", NHibernateUtil.DateTime);
                query.AddScalar("Status", NHibernateUtil.String);
                query.AddScalar("UserName", NHibernateUtil.String);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(SuUserLoginLog)));
            }

            else
            { 
                strQuery.AppendLine(" select count(*) as UserLoginLogCount ");
                strQuery.AppendLine(" from SuUserLog ");
                strQuery.AppendLine(" WHERE 1=1  ");
                
                if (!string.IsNullOrEmpty(UserName))
                {
                    strQuery.AppendLine(" and UserName like :UserName ");
                    parameterBuilder.AddParameterData("UserName", typeof(string), UserName);
                }
                    
                if (fromDate.HasValue)
                {
                    strQuery.AppendLine(" and SignInDate >= CONVERT(DATETIME, :fromDate, 103)  ");                    
                    parameterBuilder.AddParameterData("fromDate", typeof(DateTime), fromDate);
                }
                if (toDate.HasValue)
                {
                    strQuery.AppendLine(" and SignInDate < CONVERT(DATETIME, :toDate, 103) +1 "); 
                    parameterBuilder.AddParameterData("toDate", typeof(DateTime), toDate);
                }
               
                if (!string.IsNullOrEmpty(Status))
                {
                    strQuery.AppendLine(" and Status like :Status ");
                    parameterBuilder.AddParameterData("Status", typeof(string), Status);
                }              

                query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
                parameterBuilder.FillParameters(query);
               
                query.AddScalar("UserLoginLogCount", NHibernateUtil.Int32);
            }

            return query;        
        }
        public int GetCountUserLoginLoglist(DateTime? fromDate, DateTime? toDate, string UserName, string Status)
        {
            return NHibernateQueryHelper.CountByCriteria(
                QueryProvider.SuUserLogQuery,
                "FindSuUserLoginLogSearchResult",
                new object[] { fromDate, toDate, UserName,Status ,string.Empty, true });
        }

        
    }
}
