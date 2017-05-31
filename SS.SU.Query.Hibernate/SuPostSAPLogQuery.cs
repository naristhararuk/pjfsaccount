using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SS.Standard.Data.NHibernate.QueryDao;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using SS.SU.Query;
using NHibernate;
using NHibernate.Transform;
using SS.Standard.Security;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SS.SU.Query.Hibernate
{
    public class SuPostSAPLogQuery : NHibernateQueryBase<SuPostSAPLog, long>, ISuPostSAPLogQuery
    {
        public IList<SuPostSAPLogSearchResult> GetPostSAPLogList(string requestNo, string date, string status, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<SuPostSAPLogSearchResult>(
                QueryProvider.SuPostSAPLogQuery
                , "FindSuPostSAPLogSearchResult"
                , new object[] { requestNo,date,status, sortExpression, false }
                , firstResult
                , maxResult
                , sortExpression);
        }
        public int GetCountPostSAPLogList(string requestNo, string date, string status)
        {
            return NHibernateQueryHelper.CountByCriteria(
                QueryProvider.SuPostSAPLogQuery,
                "FindSuPostSAPLogSearchResult",
                new object[] { requestNo, date, status, string.Empty, true });

        }

        public ISQLQuery FindSuPostSAPLogSearchResult(string requestNo,string date,string status, string sortExpression, bool isCount)
        {
            StringBuilder strQuery = new StringBuilder();
            ISQLQuery query;

            if (isCount)
            {
                strQuery.AppendLine(" select count(*) as Count ");
                strQuery.AppendLine(" from SuPostSAPLog ");
                strQuery.AppendLine(" where RequestNo like :RequestNo ");
                strQuery.AppendLine(" and Date = coalesce(cast(nullif(:Date,'') as datetime),Date) ");
                strQuery.AppendLine(" and Status = coalesce(nullif(:Status,''),Status) ");

                query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
                query.SetString("RequestNo", "%"+requestNo+"%");
                query.SetString("Date", date);
                query.SetString("Status", status);
                query.AddScalar("Count", NHibernateUtil.Int32);

            }
            else
            {
                strQuery.AppendLine(" select Date,RequestNo,PostNo,DocumentSeqOnRequest,InvoiceNo,Year,CompanyCode,FiDocument,Flag,Message ");
                strQuery.AppendLine(" from SuPostSAPLog ");
                strQuery.AppendLine(" where RequestNo like :RequestNo ");
                strQuery.AppendLine(" and Date = coalesce(cast(nullif(:Date,'') as datetime),Date) ");
                strQuery.AppendLine(" and Status = coalesce(nullif(:Status,''),Status) ");

                if (string.IsNullOrEmpty(sortExpression))
                {
                    strQuery.Append(" order by Date,RequestNo,PostNo,DocumentSeqOnRequest,InvoiceNo,Year,CompanyCode,FiDocument,Flag,Message ");
                }
                else
                {
                    strQuery.Append(string.Format(" order by {0} ", sortExpression));
                }

                query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
                query.SetString("RequestNo", "%"+requestNo+"%");
                query.SetString("Date", date);
                query.SetString("Status", status);

                query.AddScalar("Date", NHibernateUtil.DateTime);
                query.AddScalar("RequestNo", NHibernateUtil.String);
                query.AddScalar("PostNo", NHibernateUtil.Double);
                query.AddScalar("DocumentSeqOnRequest", NHibernateUtil.Double);
                query.AddScalar("InvoiceNo", NHibernateUtil.String);
                query.AddScalar("Year", NHibernateUtil.Double);
                query.AddScalar("CompanyCode", NHibernateUtil.String);
                query.AddScalar("FiDocument", NHibernateUtil.String);
                query.AddScalar("Flag", NHibernateUtil.String);
                query.AddScalar("Message", NHibernateUtil.String);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(SuPostSAPLogSearchResult)));
            }

            return query;
        }
    }

    
}
