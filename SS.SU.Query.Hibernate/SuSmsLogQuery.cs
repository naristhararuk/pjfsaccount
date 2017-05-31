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
    public class SuSmsLogQuery : NHibernateQueryBase<SuSmsLog, long>, ISuSmsLogQuery
    {
        public SuSmsLog FindByID(long smsLogID)
        {
            return this.FindByIdentity(smsLogID);
        }
        public SuSmsLog FindBySendMsgSMID(string SMID)
        {
            StringBuilder strQuery = new StringBuilder();
            IQuery query;

           
                strQuery.AppendLine(" FROM  SuSmsLog ");
                strQuery.AppendLine(" WHERE  SendMsgSMID = :SendMsgSMID AND DlvrRepSMID IS NULL ");


                query = GetCurrentSession().CreateQuery(strQuery.ToString());
                query.SetString("SendMsgSMID", SMID);
                return query.UniqueResult<SuSmsLog>();
            

        
        }
        public IList<SuSmsLogSearchResult> GetSmsLogList(DateTime? fromDate, DateTime? toDate, string phoneNo, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<SuSmsLogSearchResult>(
                QueryProvider.SuSmsLogQuery
                , "FindSuSmsLogSearchResult"
                , new object[] { fromDate, toDate, phoneNo, sortExpression, false }
                , firstResult
                , maxResult
                , sortExpression);
        }
        public int GetCountSmsLogList(DateTime? fromDate, DateTime? toDate, string phoneNo)
        {
            return NHibernateQueryHelper.CountByCriteria(
                QueryProvider.SuSmsLogQuery,
                "FindSuSmsLogSearchResult",
                new object[] { fromDate, toDate, phoneNo, string.Empty, true });

        }

        public ISQLQuery FindSuSmsLogSearchResult(DateTime? fromDate, DateTime? toDate, string phoneNo, string sortExpression, bool isCount)
        {
            StringBuilder strQuery = new StringBuilder();
            ISQLQuery query;

            if (isCount)
            {
                strQuery.AppendLine(" select count(*) as Count ");
                strQuery.AppendLine(" from SuSmsLog ");
                //strQuery.AppendLine(" where Date >= coalesce(cast(nullif(:DateFrom,'') as datetime),Date) ");
                //strQuery.AppendLine(" and Date <= coalesce(dateadd(day,1,cast(nullif(:DateTo,'') as datetime)),Date) ");
                //strQuery.AppendLine(" and PhoneNo like :PhoneNo ");
                strQuery.AppendLine(" where PhoneNo like :PhoneNo ");

                if (fromDate != new DateTime())
                {
                    strQuery.AppendLine(" and Convert(DateTime,Convert(varchar(12),Date),103) >= Convert(DateTime,:DateFrom,103) ");
                }
                if (toDate != new DateTime())
                {
                    strQuery.AppendLine(" and Convert(DateTime,Convert(varchar(12),Date),103) < Convert(DateTime,:DateTo,103) +1 ");
                }

                query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
                //query.SetString("DateFrom", fromDate);
                //query.SetString("DateTo", toDate);
                if (fromDate != new DateTime())
                {
                    query.SetDateTime("DateFrom", fromDate.Value);
                }
                if (toDate != new DateTime())
                {
                    query.SetDateTime("DateTo", toDate.Value);
                }
                query.SetString("PhoneNo", "%" + phoneNo + "%");
                query.AddScalar("Count", NHibernateUtil.Int32);

            }
            else
            {
                strQuery.AppendLine(" select Date,PhoneNo,SendOrReceive,Message,SendMsgSMID,TRANID ");
                strQuery.AppendLine(" from SuSmsLog ");
                //strQuery.AppendLine(" where Date >= coalesce(cast(nullif(:DateFrom,'') as datetime),Date) ");
                //strQuery.AppendLine(" and Date <= coalesce(dateadd(day,1,cast(nullif(:DateTo,'') as datetime)),Date) ");
                //strQuery.AppendLine(" and PhoneNo like :PhoneNo ");
                strQuery.AppendLine(" where PhoneNo like :PhoneNo ");

                if (fromDate != new DateTime())
                {
                    strQuery.AppendLine(" and Convert(DateTime,Convert(varchar(12),Date),103) >= Convert(DateTime,:DateFrom,103) ");
                }
                if (toDate != new DateTime())
                {
                    strQuery.AppendLine(" and Convert(DateTime,Convert(varchar(12),Date),103) < Convert(DateTime,:DateTo,103) +1 ");
                }

                if (string.IsNullOrEmpty(sortExpression))
                {
                    strQuery.Append(" ORDER BY Date DESC,PhoneNo ASC,SendOrReceive ASC,Message ASC,SendMsgSMID ASC");
                }
                else
                {
                    strQuery.Append(string.Format(" order by {0} ", sortExpression));
                }

                query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
                //query.SetString("DateFrom", fromDate);
                //query.SetString("DateTo", toDate);
                if (fromDate != new DateTime())
                {
                    query.SetDateTime("DateFrom", fromDate.Value);
                }
                if (toDate != new DateTime())
                {
                    query.SetDateTime("DateTo", toDate.Value);
                }
                query.SetString("PhoneNo", "%" + phoneNo + "%");

                query.AddScalar("Date", NHibernateUtil.DateTime);
                query.AddScalar("PhoneNo", NHibernateUtil.String);
                query.AddScalar("SendOrReceive", NHibernateUtil.String);
                query.AddScalar("Message", NHibernateUtil.String);
                query.AddScalar("TRANID", NHibernateUtil.String);
                query.AddScalar("SendMsgSMID", NHibernateUtil.String);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(SuSmsLogSearchResult)));
            }

            return query;
        }
    }

    
}
