using System;
using System.Collections.Generic;
using System.Linq;
using SS.Standard.Data.NHibernate.QueryDao;
using System.Text;
using SS.SU.DTO;
using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;

namespace SS.SU.Query.Hibernate
{
    public class SuEmailResendingQuery:NHibernateQueryBase<SuEmailResending,long>,ISuEmailResendingQuery
    {
       public IList<SuEmailResending> FindAllEmailResending()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT   id, emailtype, lastsenddate, mailcontent, retry, status, credate as CreDate, sendto, creby, subject FROM SuEmailResending ");
            sqlBuilder.Append("WHERE status<>'Fail' and datediff(minute, lastsenddate,getdate()) > (select ParameterValue from dbparameter where configurationname = 'EmailPendingDuration')");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            query.AddScalar("id", NHibernateUtil.Int64);
            query.AddScalar("emailtype", NHibernateUtil.String);
            query.AddScalar("lastsenddate", NHibernateUtil.DateTime);
            query.AddScalar("mailcontent", NHibernateUtil.String);
            query.AddScalar("retry", NHibernateUtil.Int32);
            query.AddScalar("status", NHibernateUtil.String);
            query.AddScalar("CreDate", NHibernateUtil.DateTime);
            query.AddScalar("creby", NHibernateUtil.Int64);
            query.AddScalar("sendto", NHibernateUtil.String);
            query.AddScalar("subject", NHibernateUtil.String);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(SuEmailResending)));
            return query.List<SuEmailResending>();
        }

       public void DeleteSuccessItem()
       {
           string sql = "delete SuEmailResending where status = 'Success'";
           ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql);
           query.AddScalar("Count", NHibernateUtil.Int32);
           query.UniqueResult();
           
       }
    }
}
