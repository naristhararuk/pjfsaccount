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
    public class SuImageToSAPLogQuery : NHibernateQueryBase<SuImageTosapLog, long>, ISuImageToSAPLogQuery
    {
        public IList<SuImageToSAPLogSearchResult> GetImageToSAPLogList(string requestNo, DateTime? date, string status, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<SuImageToSAPLogSearchResult>(
                QueryProvider.SuImageToSAPLogQuery
                , "FindSuImageToSAPLogSearchResult"
                , new object[] { requestNo,date,status, sortExpression, false }
                , firstResult
                , maxResult
                , sortExpression);
        }
        public int GetCountImageToSAPLogList(string requestNo, DateTime? date, string status)
        {
            return NHibernateQueryHelper.CountByCriteria(
                QueryProvider.SuImageToSAPLogQuery,
                "FindSuImageToSAPLogSearchResult",
                new object[] { requestNo, date, status, string.Empty, true });

        }

        public ISQLQuery FindSuImageToSAPLogSearchResult(string requestNo, DateTime? date, string status, string sortExpression, bool isCount)
        {
            StringBuilder strQuery = new StringBuilder();
            ISQLQuery query;

            if (isCount)
            {
                strQuery.AppendLine(" select count(*) as Count ");
                strQuery.AppendLine(" from SuImageToSAPLog ");
                strQuery.AppendLine(" inner join document  on  SuImageToSAPLog.RequestNo = document.documentid");
                strQuery.AppendLine(" where document.documentno like :RequestNo ");
                //strQuery.AppendLine(" and SubmitDate = coalesce(cast(nullif(:SubmitDate,'') as datetime),SubmitDate) ");
                strQuery.AppendLine(" and Status = coalesce(nullif(:Status,''),Status) ");

                if (date != new DateTime())
                {
                    strQuery.AppendLine(" and Convert(DateTime,Convert(varchar(12),SubmitDate),103) = Convert(DateTime,:SubmitDate,103) ");
                }

                query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
                query.SetString("RequestNo", "%"+requestNo+"%");
                //query.SetString("SubmitDate", date);
                if (date != new DateTime())
                {
                    query.SetDateTime("SubmitDate", date.Value);
                }
                query.SetString("Status", status);
                query.AddScalar("Count", NHibernateUtil.Int32);

            }
            else
            {
                strQuery.AppendLine(" select document.documentno as RequestNo,SubmitDate,Status,Message ");
                strQuery.AppendLine(" from SuImageToSAPLog ");
                strQuery.AppendLine(" inner join document  on  SuImageToSAPLog.RequestNo = document.documentid");
                strQuery.AppendLine(" where document.documentno like :RequestNo ");
                //strQuery.AppendLine(" and SubmitDate = coalesce(cast(nullif(:SubmitDate,'') as datetime),SubmitDate) ");
                strQuery.AppendLine(" and Status = coalesce(nullif(:Status,''),Status) ");

                if (date != new DateTime())
                {
                    strQuery.AppendLine(" and Convert(DateTime,Convert(varchar(12),SubmitDate),103) = Convert(DateTime,:SubmitDate,103) ");
                }

                if (string.IsNullOrEmpty(sortExpression))
                {
                    strQuery.Append(" order by SubmitDate desc, RequestNo,Status,Message ");
                }
                else
                {
                    strQuery.Append(string.Format(" order by {0} ", sortExpression));
                }

                query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
                query.SetString("RequestNo", "%"+requestNo+"%");
                //query.SetString("SubmitDate", date);
                if (date != new DateTime())
                {
                    query.SetDateTime("SubmitDate", date.Value);
                }
                query.SetString("Status", status);
                
                query.AddScalar("RequestNo", NHibernateUtil.String);
                query.AddScalar("SubmitDate", NHibernateUtil.DateTime);
                query.AddScalar("Status", NHibernateUtil.String);
                query.AddScalar("Message", NHibernateUtil.String);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(SuImageToSAPLogSearchResult)));
            }

            return query;
        }
    }

    
}
