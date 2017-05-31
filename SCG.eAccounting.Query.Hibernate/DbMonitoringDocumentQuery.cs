using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.QueryDao;
using SCG.eAccounting.DTO;
using NHibernate;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.QueryCreator;
using SS.DB.Query;

namespace SCG.eAccounting.Query.Hibernate
{
    public class DbMonitoringDocumentQuery : NHibernateQueryBase<DbMonitoringDocument, string>, IDbMonitoringDocumentQuery
    {
        public int CountMonitoringDocumentQuery(string comCode, string colnumber, string BuName, short languageID, string sortExpression)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgeAccountingQueryProvider.DbMonitoringDocumentQuery, "FindDataMonitoringDocumentQuery", new object[] { comCode, colnumber, BuName, languageID, sortExpression, true });
        }

        public IList<DbMonitoringDocument> DataMonitoringDocumentQuery(string comCode, string colnumber, string BuName, short languageID, string sortExpression, int startRow, int pageSize)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<DbMonitoringDocument>(ScgeAccountingQueryProvider.DbMonitoringDocumentQuery, "FindDataMonitoringDocumentQuery", new object[] { comCode, colnumber, BuName, languageID, sortExpression, false }, startRow, pageSize, sortExpression);
        }

        public ISQLQuery FindDataMonitoringDocumentQuery(string comCode, string colnumber, string BuName, short languageID, string sortExpression, bool isCount)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();

            bool isMoreThanColumn = false;
            int from = 0;
            int to = 0;

            if (colnumber == "1")
            {
                from = Convert.ToInt32(ParameterServices.Column1_OverdueDayFrom);
                to = Convert.ToInt32(ParameterServices.Column1_OverdueDayTo);
            }
            else if (colnumber == "2")
            {
                from = Convert.ToInt32(ParameterServices.Column2_OverdueDayFrom);
                to = Convert.ToInt32(ParameterServices.Column2_OverdueDayTo);
            }
            else if (colnumber == "3")
            {
                from = Convert.ToInt32(ParameterServices.Column3_OverdueDayFrom);
                to = Convert.ToInt32(ParameterServices.Column3_OverdueDayTo);
            }
            else if (colnumber == "4")
            {
                from = Convert.ToInt32(ParameterServices.Column4_OverdueDayFrom);
                to = Convert.ToInt32(ParameterServices.Column4_OverdueDayTo);
            }
            else
            {
                isMoreThanColumn = true;
                from = Convert.ToInt32(ParameterServices.Column4_OverdueDayTo);
                //to = Convert.ToInt32(ParameterServices.Column5_OverdueDayTo);
            }

            if (isCount)
            {
                sqlBuilder.Append("SELECT count(*) as Count ");
            }
            else
            {
                sqlBuilder.Append("SELECT DocumentNo,ReferenceNo,DocumentDate,CacheCurrentStateName,Subject,CacheCreatorName,CacheRequesterName,CacheAmountLocalCurrency,CacheAmountMainCurrency,CacheAmountTHB,ApproveDate,WorkflowID,Type, CacheAttachment, CacheBoxID,ReceiveDocumentDate,IsVerifyWithImage  ");
            }
            sqlBuilder.Append(" FROM ( ");
            sqlBuilder.Append(" SELECT a.DocumentNo,a.ReferenceNo,a.DocumentDate,l.DisplayName as CacheCurrentStateName,a.Subject,a.CacheCreatorName,a.CacheRequesterName,a.CacheAmountLocalCurrency,a.CacheAmountMainCurrency,a.CacheAmountTHB,a.ApproveDate,a.WorkflowID,a.Type, isnull(a.CacheAttachment,0) as CacheAttachment, a.CacheBoxID ,a.ReceiveDocumentDate,a.IsVerifyWithImage AS IsVerifyWithImage");
            sqlBuilder.Append(" FROM [dbo].[View_Document_Mornitoring_Inbox] AS a ");
            sqlBuilder.Append(" left join WorkFlowStateLang l with (nolock) on l.WorkFlowStateID = a.CurrentState and l.LanguageID = :languageID ");
            sqlBuilder.Append(" WHERE a.CompanyCode =  coalesce(nullif(:comCode,''), a.CompanyCode) ");
            sqlBuilder.Append(" AND a.BuName = coalesce( nullif(:BuName,''), a.BuName) ");

            if (!isMoreThanColumn)
            {
                sqlBuilder.Append(" AND (Case WHEN ISNULL(a.IsVerifyWithImage,0) = 1 THEN DATEDIFF(day,a.ApproveDate, getdate()) ELSE DATEDIFF(day,a.ReceiveDocumentDate, getdate()) END) BETWEEN ");
                sqlBuilder.Append(" :fromDate AND :toDate ");
            }
            else
            {
                sqlBuilder.Append(" AND (Case WHEN ISNULL(a.IsVerifyWithImage,0) = 1 THEN DATEDIFF(day,a.ApproveDate, getdate()) ELSE DATEDIFF(day,a.ReceiveDocumentDate, getdate()) END) > :fromDate ");
            }

            sqlBuilder.Append(" ) as t1 ");


            if (!isCount)
            {
                if (!string.IsNullOrEmpty(sortExpression))
                {
                    sqlBuilder.Append(string.Format(" Order by {0} ", sortExpression));
                }
            }
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            queryParameterBuilder.AddParameterData("languageID", typeof(short), languageID);
            queryParameterBuilder.AddParameterData("comCode", typeof(string), comCode);
            queryParameterBuilder.AddParameterData("BuName", typeof(string), BuName);
            queryParameterBuilder.AddParameterData("fromDate", typeof(int), from);

            if (!isMoreThanColumn)
                queryParameterBuilder.AddParameterData("toDate", typeof(int), to);

            queryParameterBuilder.FillParameters(query);

            if (!isCount)
            {
                query.AddScalar("DocumentNo", NHibernateUtil.String)
                     .AddScalar("ReferenceNo", NHibernateUtil.String)
                     .AddScalar("DocumentDate", NHibernateUtil.DateTime)
                     .AddScalar("CacheCurrentStateName", NHibernateUtil.String)
                     .AddScalar("Subject", NHibernateUtil.String)
                     .AddScalar("CacheCreatorName", NHibernateUtil.String)
                     .AddScalar("CacheRequesterName", NHibernateUtil.String)
                     .AddScalar("CacheAmountLocalCurrency", NHibernateUtil.Double)
                     .AddScalar("CacheAmountMainCurrency", NHibernateUtil.Double)
                     .AddScalar("CacheAmountTHB", NHibernateUtil.Double)
                     .AddScalar("ApproveDate", NHibernateUtil.DateTime)
                     .AddScalar("WorkflowID", NHibernateUtil.Int64)
                     .AddScalar("Type", NHibernateUtil.String)
                     .AddScalar("CacheAttachment", NHibernateUtil.Boolean)
                     .AddScalar("CacheBoxID", NHibernateUtil.String)
                     .AddScalar("ReceiveDocumentDate", NHibernateUtil.DateTime)
                    .AddScalar("IsVerifyWithImage", NHibernateUtil.Boolean);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(DbMonitoringDocument)));
            }
            else
            {
                query.AddScalar("Count", NHibernateUtil.Int32);
                query.UniqueResult();
            }
            return query;

        }
    }
}
